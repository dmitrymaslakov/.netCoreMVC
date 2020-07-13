using System;
using System.Collections.Generic;

namespace NewsGatheringService.Domain
{
    public class NewsStructure
    {
        public Guid Id { get; set; }
        public string Headline { get; set; }
        public string Lead { get; set; }
        public string Body { get; set; }
        public string Background { get; set; }

        public Guid NewsId { get; set; }
        public News News { get; set; }
    }
}
