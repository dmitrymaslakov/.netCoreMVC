using NewsGatheringService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringService.Core.Abstract
{
    public interface IUserService
    {
        Task<Guid> RegisterUser(RegisterRequest model);
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<AuthenticateResponse> RefreshToken(string refreshToken);
    }
}
