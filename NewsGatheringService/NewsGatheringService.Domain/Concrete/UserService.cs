using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsGatheringService.Core;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NewsGatheringService.Domain.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        const string userRoleName = "user";
        private readonly AppSettings _appSettings;

        public UserService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// User authentication
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AuthenticateResponse> AuthenticateWithJwtToken(AuthenticateRequest model)
        {
            var user = _unitOfWork.UserRepository
                .FindBy(u => u.Login.Equals(model.Login))
                .FirstOrDefault();
            if (user == null) return null;

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshTokenFor(user);

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();
            
            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);

        }

        public async Task<AuthenticateResponse> RefreshToken(string refreshToken)
        {
            var tokenInDb = _unitOfWork.RefreshTokenRepository.FindBy(t => t.Token.Equals(refreshToken)).FirstOrDefault();
            var user = tokenInDb.User;
            if (user == null || !tokenInDb.IsActive)
            {
                return null;
            }
            var newRefreshToken = GenerateRefreshTokenFor(user);
            tokenInDb.Revoked = DateTime.UtcNow;
            tokenInDb.ReplacedByToken = newRefreshToken.Token;
            await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
            _unitOfWork.RefreshTokenRepository.Update(tokenInDb);
            await _unitOfWork.SaveChangesAsync();
            var jwtToken = GenerateJwtToken(user);
            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="_model"></param>
        /// <returns></returns>
        public async Task<Guid> RegisterUser(RegisterRequest _model)
        {
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Login = _model.Login,
                };

                var passwordToBytes = Encoding.UTF8.GetBytes(_model.Password);
                var enText = Convert.ToBase64String(passwordToBytes);

                user.PasswordHash = enText;

                Role userRole = _unitOfWork.RoleRepository.FindBy(r => r.Name == userRoleName).FirstOrDefault();
                if (userRole != null)
                    user.UserRoles =
                        user.UserRoles
                        .Append(new UserRole
                        { Id = Guid.NewGuid(), RoleId = userRole.Id, UserId = user.Id, Date = DateTime.Now }).ToList();

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return user.Id;
            }
            catch
            {
                throw;
            }

        }

        private string GenerateJwtToken(User user)
        {
            var role = _unitOfWork.UserRoleRepository
                .FindBy(ur => ur.UserId.CompareTo(user.Id) == 0, ur => ur.Role )
                .FirstOrDefault()?
                .Role;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private RefreshToken GenerateRefreshTokenFor(User user)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rng.GetBytes(randomBytes);
                return new RefreshToken()
                {
                    Id = Guid.NewGuid(),
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    User = user
                };
            }
        }


    }
}
