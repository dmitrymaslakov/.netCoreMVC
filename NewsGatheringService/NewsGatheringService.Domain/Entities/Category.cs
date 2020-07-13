using System;
using System.Collections.Generic;

namespace NewsGatheringService.Domain
{
    public class Category
    {
        public Category()
        {
            Subcategories = new HashSet<Subcategory>();
            NewsCategories = new HashSet<NewsCategory>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<NewsCategory> NewsCategories { get; set; }
    }
}
