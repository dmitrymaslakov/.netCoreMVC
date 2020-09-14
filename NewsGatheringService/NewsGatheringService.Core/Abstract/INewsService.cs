using NewsGatheringService.Data.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace NewsGatheringService.Core.Abstract
{
    public interface INewsService
    {
        Task InsertNewsIntoDb(SyndicationItem[] newsData);
        SyndicationItem[] GetNewsDataFromRss();
    }
}
