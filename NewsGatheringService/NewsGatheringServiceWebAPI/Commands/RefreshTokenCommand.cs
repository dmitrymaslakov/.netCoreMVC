using MediatR;
using NewsGatheringService.Models.BLL;

namespace NewsGatheringServiceWebAPI.Commands
{
    public class RefreshTokenCommand : IRequest<AuthenticateResponse>
    {
        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
        public string RefreshToken { get; set; }
    }
}
