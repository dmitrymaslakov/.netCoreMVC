﻿using Fizzler.Systems.HtmlAgilityPack;
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
    public class S13NewsParser : Is13NewsParser
    {
        public News Parse(string newsUrl)
        {
            try
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

                var headerImage = ImageConvert.ImageUrlToByte(htmlDocNode
                    .SelectSingleNode("//meta[@property='og:image']")
                    .Attributes["content"].Value);

                int.TryParse(htmlDocNode
                    .SelectSingleNode("//span[@title='Просмотров']")
                    .InnerText
                    .Replace("/n", "").Replace(" ", ""), out var views);

                var category = "Без категории";

                var subcategory = "";

                var headline = htmlDocNode
                    .QuerySelector("h1")
                    .InnerText;

                headline = Regex.Replace(headline, RegexPattern.Pattern, " ").Trim();


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
