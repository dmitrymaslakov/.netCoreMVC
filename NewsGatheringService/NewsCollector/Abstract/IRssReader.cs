using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace NewsCollector.Abstract
{
    public interface IRssReader
    {
        IEnumerable<SyndicationItem> GetNewsDataFromRssFeed(string feed);
    }
}
