using NewsGatheringService.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace NewsGatheringService.DAL.Entities
{
    public class Subcategory : IEntity
    {
        public Subcategory()
        {
            //NewsCategories = new HashSet<NewsCategory>();
            News = new HashSet<News>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        //public IEnumerable<NewsCategory> NewsCategories { get; set; }
        public IEnumerable<News> News { get; set; }
    }
}
