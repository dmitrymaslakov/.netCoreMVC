using NewsCollector.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsCollector.Concrete
{
    public class RssReader : IRssReader
    {
        public IEnumerable<SyndicationItem> GetNewsDataFromRssFeed(string feedUrl)
        {
            try
            {
                using (var reader = XmlReader.Create(feedUrl))
                {
                    var feed = SyndicationFeed.Load(reader);
                    foreach (var item in feed.Items)
                    {
                        item.SourceFeed = feed;
                    }
                    var news = feed.Items.ToArray();
                    return news;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
