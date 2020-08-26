using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NewsGatheringService.Domain.Onliner
{
    public class OnlinerNewsParser : IParser<News>
    {
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
        public News Parse(IHtmlDocument document)
        {
            DateTime.TryParse(document
                .QuerySelector("meta[name='article:published_time']")
                .GetAttribute("content"), out var date);

            var source = document
                .QuerySelector("meta[property='og:url']")
                .GetAttribute("content");

            var author = document
                .QuerySelector("meta[name='author']")
                .GetAttribute("content");

            var headerImage = ImageUrlToByte(document
                .QuerySelector("meta[name='twitter:image']")
                .GetAttribute("content"));

            int.TryParse(document
                .QuerySelector("div.news-header__button_views")
                .TextContent.Replace("/n", "").Replace(" ", ""), out var views);

            var reputation = NewsEstimate(views);

            var category = document
                .QuerySelector("li.project-navigation__item_active")
                .GetElementsByClassName("project-navigation__sign")
                .FirstOrDefault()?
                .TextContent;

            var subcategory = document
                .QuerySelector("a.news-reference__link_primary")?
                .TextContent.Replace("/n", "").Trim();

            var headline = document
                .QuerySelector("h1")
                .TextContent;

            var lead = document
                .QuerySelector("div.news-entry__speech")?
                .GetElementsByTagName("p")
                .FirstOrDefault()?
                .TextContent;

            var divNewsEntry = document
                //.QuerySelector("div.news-entry");
                .QuerySelector("div.news-text")
                .FirstElementChild;

            var divNewsWidget = document
                .QuerySelector("div.news-widget");
            var body = new StringBuilder();

            var nextElement = divNewsEntry?.NextElementSibling;
            do
            {
                if (nextElement.LocalName.Equals("p") || nextElement.LocalName.Equals("h2"))
                {
                    body.Append(nextElement.OuterHtml);
                }
                nextElement = nextElement.NextElementSibling;

            } while (nextElement != null);

            var news = new News
            {
                Date = date,
                Source = source,
                Author = author,
                NewsHeaderImage = headerImage,
                Reputation = reputation,
                NewsStructure = new NewsStructure
                {
                    Headline = headline,
                    Lead = lead,
                    Body = body.ToString()
                },
                Category= new Category { Name = category },
                Subcategory = new Subcategory { Name = subcategory }
                /*NewsCategory = 
                    new NewsCategory
                    {
                        Subcategory = new Subcategory { Name = subcategory },
                        Category = new Category { Name = category }
                    }      */
            };

            return news;
        }
    }
}
