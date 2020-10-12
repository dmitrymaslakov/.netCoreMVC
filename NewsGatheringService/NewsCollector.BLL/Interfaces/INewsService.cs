using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Interfaces
{
    public interface INewsService
    {
        /// <summary>
        /// Take unparsed urls from NewsUrl table, parse them and insert into db
        /// </summary>
        /// <returns></returns>
        Task ParseNewsAndInsertIntoDb();
        /// <summary>
        /// Insert news urls only those that are not in the database;
        /// </summary>
        /// <returns></returns>
        Task InsertNewsUrlsToDb(string[] newsUrls);
        /// <summary>
        /// Get news data from all rss feeds that is not in the database
        /// </summary>
        /// <returns></returns>
        SyndicationItem[] GetNewsData();
        /// <summary>
        /// Perform news evaluation and update database
        /// </summary>
        /// <param name="newsUrls"></param>
        /// <returns></returns>
        Task PerformNewsEvaluationAsync();
    }
}
