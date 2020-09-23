using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace NewsGatheringService.MVC.PL.Models
{
    public class RecentNews
    {
        const string s13 = "s13.ru";
        const string onliner = "onliner.by";
        const string tut = "tut.by";
        [Display(Name = "Заголовок")]
        public string Title { get; set; }
        [Display(Name = "Источник")]
        public string Source { get; set; }
        [Display(Name = "Дата публикации")]
        public DateTime Date { get; set; }
        public string SourceUrl { get; set; }
        public RecentNews(SyndicationItem syndicationItem)
        {
            Title = syndicationItem.Title.Text;
            if (syndicationItem.Id.Contains(onliner))
                Source = onliner;
            else if (syndicationItem.Id.Contains(s13))
                Source = s13;
            else if (syndicationItem.Id.Contains(tut))
                Source = tut;
            else
            {
                Source = "";
            }
            SourceUrl = syndicationItem.Id;
            Date = syndicationItem.PublishDate.DateTime;
        }
    }
}
