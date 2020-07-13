using System;
using System.Collections.Generic;

namespace NewsGatheringService.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public Guid NewsId { get; set; }

        public News News { get; set; }
        public User User { get; set; }
    }
}
