using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Interfaces
{
    public interface IRssReader
    {
        /// <summary>
        /// Get news data from rss feed
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        IEnumerable<SyndicationItem> GetNewsDataFromRssFeed(string feed);
    }
}
