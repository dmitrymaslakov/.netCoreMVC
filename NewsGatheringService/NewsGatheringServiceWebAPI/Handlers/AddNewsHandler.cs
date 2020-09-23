using MediatR;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringServiceWebAPI.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class AddNewsHandler : IRequestHandler<AddNewsCommand, bool>
    {
        private readonly INewsService _newsService;

        public AddNewsHandler(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<bool> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _newsService.AddRecentNewsToDbAsync(request.LatestNewsUrls);
            }
            catch
            {
                throw;
            }
        }
    }
}
