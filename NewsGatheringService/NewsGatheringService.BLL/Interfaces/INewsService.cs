using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace NewsGatheringService.BLL.Interfaces
{
    public interface INewsService
    {
        Task InsertNewsIntoDb(SyndicationItem[] newsData);
        /// <summary>
        /// Get all news data from rss
        /// </summary>
        /// <returns></returns>
        SyndicationItem[] GetNewsDataFromRss();
        /// <summary>
        /// Get news data from rss that is not in the database
        /// </summary>
        /// <returns></returns>
        SyndicationItem[] GetRecentNewsDataFromRss();
        /// <summary>
        /// Get news data from rss that is not in the database and add them to the database
        /// </summary>
        /// <param name="newsUrls"></param>
        /// <returns></returns>
        Task<bool> AddRecentNewsToDbAsync(string[] newsUrls = null);
    }
}
