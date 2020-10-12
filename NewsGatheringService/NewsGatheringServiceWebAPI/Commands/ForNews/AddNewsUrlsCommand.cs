using MediatR;

namespace NewsGatheringServiceWebAPI.Commands
{
    public class AddNewsUrlsCommand : IRequest<Unit>
    {
        public AddNewsUrlsCommand(string[] latestNewsUrls)
        {
            LatestNewsUrls = latestNewsUrls;
        }
        public string[] LatestNewsUrls { get; set; }
    }
}
