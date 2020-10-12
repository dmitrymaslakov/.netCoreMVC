using MediatR;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.Models.BLL;
using NewsGatheringServiceWebAPI.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthenticateResponse>
    {
        private readonly IUserService _userService;

        public RefreshTokenHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AuthenticateResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RefreshTokenAsync(request.RefreshToken);
        }
    }
}
