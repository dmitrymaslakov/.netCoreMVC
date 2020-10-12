using MediatR;
using NewsGatheringService.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Commands
{
    public class RegistrateUserCommand : IRequest<AuthenticateResponse>
    {
        public RegistrateUserCommand(RegisterRequest request)
        {
            Request = request;
        }
        public RegisterRequest Request { get; set; }
    }
}
