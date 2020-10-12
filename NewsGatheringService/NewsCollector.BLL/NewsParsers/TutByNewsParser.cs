using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using NewsCollector.BLL.Helpers;
using NewsCollector.BLL.Interfaces;
using NewsGatheringService.DAL.Entities;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NewsCollector.BLL.NewsParsers
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
                    .SelectSingleNode("//meta[@property='article:published_time']")
                    ?.Attributes["content"].Value, out var date);

                var source = newsUrl;

                var author = string.Join("/", htmlDocNode
                    .SelectNodes("//span[@itemprop='author']")
                    .Select(n => n.SelectSingleNode("//span[@itemprop='name']"))
                    .FirstOrDefault()?.InnerText);
                    

                if (string.IsNullOrEmpty(author))
                {
                    author = htmlDocNode
                        .SelectNodes("//span[@itemprop='author']")
                        .Select(n => n.SelectSingleNode("//meta[@itemprop='name']"))
                        .FirstOrDefault()
                        ?.Attributes["content"]
                        .Value;
                }

                var headerImage = ImageConvert.ImageUrlToByte(htmlDocNode
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

                var category = htmlDocNode
                    .QuerySelector("div.b-nav")?
                    .QuerySelector("li.active")?
                    .QuerySelector("a")?
                    .InnerText ?? "Без категории";

                var subcategory = "";

                var headline = htmlDocNode
                    .QuerySelector("h1")
                    .InnerText;

                headline = Regex.Replace(headline, RegexPattern.Pattern, " ").Trim();

                var lead = "";

                var pOrh2Elements = htmlDocNode
                    .SelectSingleNode("//div[@id='article_body']")?
                    .FirstChild;

                var body = new StringBuilder();

                var nextElement = pOrh2Elements?.NextSibling;

                do
                {
                    var elName = nextElement.Name;
                    if (nextElement.Name.Equals("p") || nextElement.Name.Equals("h2"))
                    {
                        body.Append(nextElement.OuterHtml);
                    }
                    nextElement = nextElement.NextSibling;

                } while (nextElement != null);

                var bodyStr = Regex.Replace(body.ToString(), RegexPattern.Pattern, " ").Trim();

                var news = new News
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    Source = new NewsUrl { Url = source },
                    Author = author,
                    NewsHeaderImage = headerImage,
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
    }
}
