using MediatR;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.Models.BLL;
using NewsGatheringServiceWebAPI.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateResponse>
    {
        private readonly IUserService _userService;

        public AuthenticateUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AuthenticateResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.AuthenticateWithJwtTokenAsync(request.Request);
        }
    }
}
