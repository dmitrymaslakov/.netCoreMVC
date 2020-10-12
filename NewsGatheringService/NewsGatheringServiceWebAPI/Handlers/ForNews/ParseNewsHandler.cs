using MediatR;
using NewsCollector.BLL.Interfaces;
using NewsGatheringServiceWebAPI.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class ParseNewsHandler : IRequestHandler<ParseNewsCommand, Unit>
    {
        private readonly INewsService _newsService;

        public ParseNewsHandler(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<Unit> Handle(ParseNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _newsService.ParseNewsAndInsertIntoDb();

                return new Unit();
            }
            catch
            {
                throw;
            }
        }

    }
}
