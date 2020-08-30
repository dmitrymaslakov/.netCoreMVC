using NewsGatheringService.Data.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace NewsCollector.Abstract
{
    public interface INewsService
    {
        /*Task GetDataFromRssAndInsertIntoDb();
        SyndicationItem[] GetDataFromRss();*/
        //void AttemptedToDivideByZero();
        Task InsertNewsIntoDb(SyndicationItem[] newsData);
        SyndicationItem[] GetNewsDataFromRss();
    }
}
