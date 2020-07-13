using System;
using System.Collections.Generic;

namespace NewsGatheringService.Domain
{
    public class Subcategory
    {
        public Subcategory()
        {
            NewsCategories = new HashSet<NewsCategory>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<NewsCategory> NewsCategories { get; set; }
    }
}
