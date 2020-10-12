using NewsGatheringService.DAL.Interfaces;
using System;

namespace NewsGatheringService.DAL.Entities
{
    public class NewsUrl : IEntity
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Guid? NewsId { get; set; }
        public News News { get; set; }
    }
}
