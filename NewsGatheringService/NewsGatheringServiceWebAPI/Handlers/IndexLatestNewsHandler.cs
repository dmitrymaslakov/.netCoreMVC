using MediatR;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.Models.BLL;
using NewsGatheringServiceWebAPI.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class IndexLatestNewsHandler : IRequestHandler<IndexLatestNewsQuery, IEnumerable<RecentNews>>
    {
        private readonly INewsService _newsService;

        public IndexLatestNewsHandler(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IEnumerable<RecentNews>> Handle(IndexLatestNewsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var recentDataNews = _newsService.GetRecentNewsDataFromRss();

                var recentNews = recentDataNews.Select(i => new RecentNews(i));
                return recentNews;
            }
            catch
            {
                throw;
            }
        }
    }
}
