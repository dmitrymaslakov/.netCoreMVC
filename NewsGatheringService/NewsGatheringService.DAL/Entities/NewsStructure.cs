using NewsGatheringService.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace NewsGatheringService.DAL.Entities
{
    public class NewsStructure : IEntity
    {
        public Guid Id { get; set; }
        public string Headline { get; set; }
        public string Lead { get; set; }
        public string Body { get; set; }

        public Guid NewsId { get; set; }
        public News News { get; set; }
    }
}
