using MediatR;
using NewsCollector.BLL.Interfaces;
using NewsGatheringServiceWebAPI.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class RateNewsHandler : IRequestHandler<RateNewsCommand, Unit>
    {
        private readonly INewsService _newsService;

        public RateNewsHandler(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<Unit> Handle(RateNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _newsService.PerformNewsEvaluationAsync();

                return new Unit();
            }
            catch
            {
                throw;
            }
        }

    }
}
