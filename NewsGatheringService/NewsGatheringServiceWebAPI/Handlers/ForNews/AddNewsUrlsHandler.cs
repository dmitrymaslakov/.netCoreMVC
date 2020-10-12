using MediatR;
using NewsCollector.BLL.Interfaces;
using NewsGatheringServiceWebAPI.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class AddNewsUrlsHandler : IRequestHandler<AddNewsUrlsCommand, Unit>
    {
        private readonly INewsService _newsService;

        public AddNewsUrlsHandler(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<Unit> Handle(AddNewsUrlsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _newsService.InsertNewsUrlsToDb(request.LatestNewsUrls);

                return new Unit();
            }
            catch
            {
                throw;
            }
        }

    }
}
