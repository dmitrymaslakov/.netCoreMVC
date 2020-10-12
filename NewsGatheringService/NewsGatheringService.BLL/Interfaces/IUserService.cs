using NewsGatheringService.DAL.Entities;
using NewsGatheringService.Models.BLL;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewsGatheringService.BLL.Interfaces
{
    /// <summary>
    /// Class for user service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="_model"></param>
        /// <returns></returns>
        Task<Guid?> RegisterUserAsync(RegisterRequest model);

        /// <summary>
        /// User authentication based on Jwt Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticateResponse> AuthenticateWithJwtTokenAsync(AuthenticateRequest model);
        
        /// <summary>
        /// User authentication based on cookie 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ClaimsIdentity AuthenticateWithCookie(AuthenticateRequest model);

        /// <summary>
        /// Admin registration
        /// </summary>
        /// <param name="_model"></param>
        /// <returns></returns>
        Task RegisterAdminAsync();

        /// <summary>
        /// Refresh user jwt token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<AuthenticateResponse> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Adding roles
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<Role> AddRolesToDbAsync(string roleName);
    }
}
