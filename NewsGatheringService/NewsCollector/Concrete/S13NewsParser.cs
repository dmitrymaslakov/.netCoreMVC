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
    public class S13NewsParser : Is13NewsParser
    {
        public News Parse(string newsUrl)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(newsUrl);

            var htmlDocNode = htmlDoc.DocumentNode;

            var b = htmlDocNode
                .QuerySelector("ul.cols.top")
                .QuerySelectorAll("span")
                .Where(e => !e.HasAttributes)
                .FirstOrDefault()?
                .InnerText;

            DateTime.TryParse(htmlDocNode
                .QuerySelector("ul.cols.top")
                .QuerySelectorAll("span")
                .Where(e => !e.HasAttributes)
                .FirstOrDefault()?
                .InnerText, out var date);

            var source = newsUrl;

            var author = "s13.ru";

            var headerImage = ImageUrlToByte(htmlDocNode
                .SelectSingleNode("//meta[@property='og:image']")
                .Attributes["content"].Value);

            int.TryParse(htmlDocNode
                .SelectSingleNode("//span[@title='Просмотров']")
                .InnerText
                .Replace("/n", "").Replace(" ", ""), out var views);

            var reputation = NewsEstimate(views);

            var category = "";
            
            var subcategory = "";

            var headline = htmlDocNode
                .QuerySelector("h1")
                .InnerText;

            headline = Regex.Replace(headline, "<[^>]+>|&nbsp;", string.Empty).Trim();


            var lead = "";

            var body = new StringBuilder();

            var nextElement = htmlDocNode
                .QuerySelector("h1")
                .NextSibling;
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
