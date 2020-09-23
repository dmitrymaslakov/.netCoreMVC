using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace NewsCollector.BLL.Interfaces
{
    public interface IRssReader
    {
        IEnumerable<SyndicationItem> GetNewsDataFromRssFeed(string feed);
    }
}
