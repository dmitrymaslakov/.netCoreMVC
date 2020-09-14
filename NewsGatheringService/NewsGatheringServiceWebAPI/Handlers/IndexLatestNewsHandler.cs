using MediatR;
using NewsCollector.Abstract;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Models;
using NewsGatheringServiceWebAPI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class IndexLatestNewsHandler : IRequestHandler<IndexLatestNewsQuery, IEnumerable<RecentNews>>
    {
        private readonly IRepository<News> _newsRepository;
        private readonly INewsService _newsService;

        public IndexLatestNewsHandler(IRepository<News> newsRepository, INewsService newsService)
        {
            _newsRepository = newsRepository;
            _newsService = newsService;
        }

        public async Task<IEnumerable<RecentNews>> Handle(IndexLatestNewsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var recentDataNews = new List<SyndicationItem>();
                var newsDb = //await 
                    _newsRepository.GetAllAsync();

                foreach (var syndicationItem in _newsService.GetNewsDataFromRss())
                {
                    if (newsDb.Any(n => n.Source.Equals(syndicationItem.Id)))
                        continue;
                    recentDataNews.Add(syndicationItem);
                }
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
