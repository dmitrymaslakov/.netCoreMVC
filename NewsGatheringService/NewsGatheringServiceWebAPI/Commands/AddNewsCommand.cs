using MediatR;
using System.ServiceModel.Syndication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Commands
{
    public class AddNewsCommand : IRequest<bool>
    {
        public AddNewsCommand(string[] latestNewsUrls)
        {
            LatestNewsUrls = latestNewsUrls;
        }
        public string[] LatestNewsUrls { get; set; }
    }
}
