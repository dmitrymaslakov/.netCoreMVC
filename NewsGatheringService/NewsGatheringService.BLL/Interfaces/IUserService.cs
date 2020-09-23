using NewsGatheringService.DAL.Entities;
using NewsGatheringService.Models.BLL;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewsGatheringService.BLL.Interfaces
{
    public interface IUserService
    {
        Task<Guid> RegisterUserAsync(RegisterRequest model);
        Task<AuthenticateResponse> AuthenticateWithJwtToken(AuthenticateRequest model);
        ClaimsIdentity AuthenticateWithCookie(AuthenticateRequest model);
        Task RegisterAdminAsync();
        Task<AuthenticateResponse> RefreshToken(string refreshToken);
        Task<Role> AddRolesToDbAsync(string roleName);
    }
}
