using AngleSharp.Browser;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using NewsGatheringService.Domain;
using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsGatheringService.Domain.Onliner
{
    public class OnlinerNewsUrlSourceParser : IParser<IEnumerable<string>>
    {
        public IEnumerable<string> Parse(IHtmlDocument document)
        {
            var newsUrlSourceList = new HashSet<string>();
            //Список блоков <div> с новостями
            var items = document
                .QuerySelectorAll("div")
                .Where(item => item.ClassName != null && item.ClassName.Contains("b-main-page-grid-4 b-main-page-news-2"))
                .SelectMany(item => item.Children
                    .Where(itemChild => itemChild.ClassName.Equals("b-main-page-grid-4-i sep-lines-type-1 cfix"))
                    .Select(itemChild => item));

            foreach (var item in items)
            {
                //блок новостей одной категории
                var divNews = item.GetElementsByClassName("b-main-page-grid-4-i sep-lines-type-1 cfix").FirstOrDefault();

                foreach (var divMainNews in divNews.Children)
                {
                    var urlSource = divMainNews
                        .GetElementsByClassName("b-section-main__col-fig")
                        .FirstOrDefault()?
                        .ParentElement
                        .GetAttribute("href");
                    newsUrlSourceList.Add(urlSource);
                }
            }

            return newsUrlSourceList;
        }
    }
}
