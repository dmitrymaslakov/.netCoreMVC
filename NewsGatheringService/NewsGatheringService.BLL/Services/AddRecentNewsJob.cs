using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.DAL.Interfaces;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace NewsGatheringService.BLL.Services
{
    public class AddRecentNewsJob : IAddRecentNewsJob
    {
        private readonly IRepository<News> _newsRepository;
        private readonly INewsService _newsService;
        private readonly List<SyndicationItem> recentDataNews = new List<SyndicationItem>();
        public AddRecentNewsJob(IRepository<News> newsRepository, INewsService newsService)
        {
            _newsRepository = newsRepository;
            _newsService = newsService;
        }

        public async Task AddNews()
        {
            await _newsService.AddRecentNewsToDbAsync();
            /*var newsDb = _newsRepository.GetAllAsync();

            foreach (var syndicationItem in _newsService.GetNewsDataFromRss())
            {
                if (newsDb.Any(n => n.Source.Equals(syndicationItem.Id)))
                    continue;
                recentDataNews.Add(syndicationItem);
            }
            await _newsService.InsertNewsIntoDb(recentDataNews.ToArray());*/
        }
    }
}
