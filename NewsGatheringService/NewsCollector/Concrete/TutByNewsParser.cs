using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using NewsCollector.Abstract;
using NewsGatheringService.Data.Entities;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NewsCollector.Concrete
{
    public class TutByNewsParser : ITutByNewsParser
    {

        public News Parse(string newsUrl)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = web.Load(newsUrl);

                var htmlDocNode = htmlDoc.DocumentNode;

                DateTime.TryParse(htmlDocNode
                    .SelectSingleNode("//meta[@property='article:published_time']")?
                    .Attributes["content"].Value, out var date);

                var source = newsUrl;

                var author = string.Join("/", htmlDocNode
                    .SelectNodes("//span[@itemprop='author']")
                    .Select(n => n.SelectSingleNode("//span[@itemprop='name']"))
                    .Select(n => n.InnerText));



                var headerImage = ImageUrlToByte(htmlDocNode
                    .SelectSingleNode("//meta[@property='twitter:image']")
                    .Attributes["content"].Value);

                int.TryParse(htmlDocNode
                    .SelectNodes("//div[@class='rate-button']")?
                    .Where(n =>
                    {
                        var s = n.QuerySelector("span.icon-smile-like");
                        return s != null;
                    })?
                    .FirstOrDefault()?
                    .QuerySelector("span.rate-value")?
                    .InnerText, out var views);

                var reputation = NewsEstimate(views);

                var category = htmlDocNode
                    .QuerySelector("div.b-nav")?
                    .QuerySelector("li.active")?
                    .QuerySelector("a")?
                    .InnerText ?? "Без категории";

                var subcategory = "";

                var headline = htmlDocNode
                    .QuerySelector("h1")
                    .InnerText;

                headline = Regex.Replace(headline, "<[^>]+>|&nbsp;|&laquo;|&raquo;|&mdash;|&bdquo;|&ldquo;", " ").Trim();

                var lead = "";

                var pOrh2Elements = htmlDocNode
                    .SelectSingleNode("//div[@id='article_body']")
                    .FirstChild;

                var body = new StringBuilder();

                var nextElement = pOrh2Elements?.NextSibling;
                do
                {
                    if (nextElement.Name.Equals("p") || nextElement.Name.Equals("h2"))
                    {
                        body.Append(nextElement.OuterHtml);
                    }
                    nextElement = nextElement.NextSibling;

                } while (nextElement != null);


                var bodyStr = Regex.Replace(body.ToString(), "<[^>]+>", string.Empty);

                var news = new News
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    Source = source,
                    Author = author,
                    NewsHeaderImage = headerImage,
                    Reputation = reputation,
                    NewsStructure = new NewsStructure
                    {
                        Id = Guid.NewGuid(),
                        Headline = headline,
                        Lead = string.IsNullOrEmpty(lead) ? "" : lead,
                        Body = bodyStr
                    },
                    Category = new Category { Id = Guid.NewGuid(), Name = category },
                    Subcategory = new Subcategory { Id = Guid.NewGuid(), Name = subcategory }
                };

                return news;
            }
            catch
            {

                throw;
            }

        }

        private byte[] ImageUrlToByte(string imageUrl)
        {
            var stream = new WebClient().OpenRead(imageUrl);
            var dataImage = new byte[0];
            using (var streamReader = new MemoryStream())
            {
                stream.CopyTo(streamReader);
                dataImage = streamReader.ToArray();
            }
            return dataImage;
        }
        private int NewsEstimate(int views)
        {
            return views > 10 && views < 50 ? 1 :
                (views >= 50 && views < 500 ? 2 :
                    (views >= 500 && views < 5000 ? 3 :
                        (views >= 5000 && views < 15000 ? 4 :
                            (views >= 15000 ? 5 : -1))));
        }

    }
}
