using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using NewsCollector.BLL.Helpers;
using NewsCollector.BLL.Interfaces;
using NewsGatheringService.DAL.Entities;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewsCollector.BLL.NewsParsers
{
    public class OnlinerNewsParser : IOnlinerNewsParser
    {
        public async Task<News> ParseAsync(string newsUrl)
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

                var author = htmlDocNode
                    .SelectSingleNode("//meta[@name='author']")
                    .Attributes["content"].Value;

                var headerImage = ImageUrlToByte(htmlDocNode
                    .SelectSingleNode("//meta[@name='twitter:image']")
                    .Attributes["content"].Value);

                int.TryParse(htmlDocNode
                    .QuerySelector("div.news-header__button_views")
                    .InnerText
                    .Replace("/n", "").Replace(" ", ""), out var views);

                var category = htmlDocNode
                    .QuerySelector("li.project-navigation__item_active")
                    .QuerySelector("span.project-navigation__sign")
                    .InnerText;

                var subcategory = htmlDocNode
                    .QuerySelector("a.news-reference__link_primary")?
                    .InnerText.Replace("/n", "").Trim();

                var headline = htmlDocNode
                    .QuerySelector("h1")
                    .InnerText;

                headline = Regex.Replace(headline, RegexPattern.Pattern, " ").Trim();

                var lead = htmlDocNode
                    .QuerySelector("div.news-entry__speech")?
                    .Element("p")?
                    .InnerText;

                var pOrh2Elements = htmlDocNode
                    .QuerySelector("div.news-text")
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

                var bodyStr = Regex.Replace(body.ToString(), RegexPattern.Pattern, " ").Trim();

                var news = new News
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    Source = source,
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
                news.Reputation = await NewsEvaluation.EvaluateNewsAsync(news);
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
    }
}
