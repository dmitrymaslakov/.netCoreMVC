using MediatR;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.Models.BLL;
using NewsGatheringServiceWebAPI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class RegistrateUserHandler : IRequestHandler<RegistrateUserCommand, AuthenticateResponse>
    {
        private readonly IUserService _userService;

        public RegistrateUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AuthenticateResponse> Handle(RegistrateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userService.RegisterUserAsync(request.Request);

            if (userId != null)
                return await _userService.AuthenticateWithJwtTokenAsync(
                    new AuthenticateRequest { Login = request.Request.Login, Password = request.Request.Password });
           
            else return null;
        }
    }
}
