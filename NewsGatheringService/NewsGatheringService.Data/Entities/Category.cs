﻿using NewsGatheringService.Data.Abstract;
using System;
using System.Collections.Generic;

namespace NewsGatheringService.Data.Entities
{
    public class Category : IEntity
    {
        public Category()
        {
            Subcategories = new HashSet<Subcategory>();
            News = new HashSet<News>();
            //NewsCategories = new HashSet<NewsCategory>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        //public IEnumerable<NewsCategory> NewsCategories { get; set; }
        public IEnumerable<News> News { get; set; }
    }
}