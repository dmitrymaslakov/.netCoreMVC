using MediatR;
using NewsGatheringService.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Commands
{
    public class AuthenticateUserCommand : IRequest<AuthenticateResponse>
    {
        public AuthenticateUserCommand(AuthenticateRequest request)
        {
            Request = request;
        }
        public AuthenticateRequest Request { get; set; }

    }
}
