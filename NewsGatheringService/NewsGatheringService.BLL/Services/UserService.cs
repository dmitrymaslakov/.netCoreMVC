using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.DAL.Models;
using NewsGatheringService.Models.BLL;
using NewsGatheringService.UOW.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;


namespace NewsGatheringService.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        const string userRoleName = "user";
        const string adminRoleName = "admin";

        private readonly AppSettings _appSettings;

        public UserService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;

            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> AuthenticateWithJwtTokenAsync(AuthenticateRequest model)
        {
            try
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
            catch
            {
                throw;
            }
        }

        public ClaimsIdentity AuthenticateWithCookie(AuthenticateRequest model)
        {
            try
            {
                var user = _unitOfWork.UserRepository
                    .FindBy(u => u.Login.Equals(model.Login), u => u.UserRoles)
                    .FirstOrDefault();

                if (user == null || !BC.Verify(model.Password, user.PasswordHash))
                    return null;

                var role = _unitOfWork.UserRoleRepository
                    .FindBy(ur => ur.UserId.CompareTo(user.Id) == 0, ur => ur.Role)
                    .FirstOrDefault()?
                    .Role;

                var login = user.Login;

                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),

                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
            };

                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return id;
            }
            catch
            {
                throw;
            }
        }

        public async Task<AuthenticateResponse> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var tokenInDb = _unitOfWork.RefreshTokenRepository
                    .FindBy(t => t.Token.Equals(refreshToken), t => t.User)
                    .FirstOrDefault();

                if (tokenInDb == null)
                    return null;

                var user = tokenInDb.User;

                if (user == null || !tokenInDb.IsActive)
                    return null;

                var newRefreshToken = GenerateRefreshTokenFor(user);

                tokenInDb.Revoked = DateTime.UtcNow;

                tokenInDb.ReplacedByToken = newRefreshToken.Token;

                await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);

                _unitOfWork.RefreshTokenRepository.Update(tokenInDb);

                await _unitOfWork.SaveChangesAsync();

                var jwtToken = GenerateJwtToken(user);

                return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Guid?> RegisterUserAsync(RegisterRequest model)
        {
            try
            {
                var user = _unitOfWork.UserRepository?
                    .FindBy(u => u.Login.Equals(model.Login), u => u.UserRoles)
                    .FirstOrDefault();

                if (user != null)
                    return null;

                user = new User
                {
                    Id = Guid.NewGuid(),
                    Login = model.Login,
                    PasswordHash = BC.HashPassword(model.Password)
                };

                var userRole = _unitOfWork.RoleRepository.FindBy(r => r.Name == userRoleName).FirstOrDefault();

                if (userRole == null)
                    userRole = await AddRolesToDbAsync(userRoleName);

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

        public async Task RegisterAdminAsync()
        {
            try
            {
                var adminLogin = "adminLogin";

                var adminPassword = "123456";

                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Login = adminLogin,
                    PasswordHash = BC.HashPassword(adminPassword)
                };

                var adminRole = await AddRolesToDbAsync(adminRoleName);

                adminUser.UserRoles = adminUser.UserRoles
                    .Append(new UserRole
                    { Id = Guid.NewGuid(), RoleId = adminRole.Id, UserId = adminUser.Id, Date = DateTime.Now }).ToList();

                await _unitOfWork.UserRepository.AddAsync(adminUser);

                await _unitOfWork.SaveChangesAsync();

            }
            catch
            {
                throw;
            }

        }

        public async Task<Role> AddRolesToDbAsync(string roleName)
        {
            try
            {
                var role = new Role { Id = Guid.NewGuid(), Name = roleName };

                await _unitOfWork.RoleRepository.AddAsync(role);

                await _unitOfWork.SaveChangesAsync();

                return role;
            }
            catch
            {
                throw;
            }
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                var role = _unitOfWork.UserRoleRepository
                    .FindBy(ur => ur.UserId.CompareTo(user.Id) == 0, ur => ur.Role)
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
            catch
            {
                throw;
            }
        }

        private RefreshToken GenerateRefreshTokenFor(User user)
        {
            try
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
            catch
            {
                throw;
            }
        }
    }
}
