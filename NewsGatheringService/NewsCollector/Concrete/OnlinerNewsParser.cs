﻿using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using NewsCollector.Abstract;
using NewsGatheringService.Data.Entities;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NewsCollector.Concrete
{
    public class OnlinerNewsParser : IOnlinerNewsParser
    {

        public News Parse(string newsUrl)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(newsUrl);

            var htmlDocNode = htmlDoc.DocumentNode;

            DateTime.TryParse(htmlDocNode
                .SelectSingleNode("//meta[@name='article:published_time']")
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

            var reputation = NewsEstimate(views);

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

            headline = Regex.Replace(headline, "<[^>]+>|&nbsp;", string.Empty).Trim();

            var lead = htmlDocNode
                .QuerySelector("div.news-entry__speech")?
                .Element("p")?
                .InnerText;

            //lead = Regex.Replace(lead, @"<[^>]+>|&nbsp;", "").Trim();

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