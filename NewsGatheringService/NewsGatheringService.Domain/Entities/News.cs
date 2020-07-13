using NewsGatheringService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsGatheringService.Domain
{
    public class News
    {
        public News()
        {
            NewsCategories = new HashSet<NewsCategory>();
            Comments = new HashSet<Comment>();
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public string Author { get; set; }
        public byte[] NewsHeaderImage { get; set; }
        public int Reputation { get; set; }

        public NewsStructure NewsStructure { get; set; }
        public IEnumerable<NewsCategory> NewsCategories { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
