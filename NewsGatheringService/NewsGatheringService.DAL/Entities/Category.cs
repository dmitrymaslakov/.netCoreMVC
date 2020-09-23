using NewsGatheringService.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace NewsGatheringService.DAL.Entities
{
    public class Category : IEntity
    {
        public Category()
        {
            Subcategories = new HashSet<Subcategory>();
            News = new HashSet<News>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<News> News { get; set; }
    }
}
