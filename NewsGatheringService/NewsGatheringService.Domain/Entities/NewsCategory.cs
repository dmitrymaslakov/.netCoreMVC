using System;
using System.Collections.Generic;
using System.Text;

namespace NewsGatheringService.Domain
{
    public class NewsCategory
    {
        public Guid Id { get; set; }
        public Guid NewsId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubcategoryId { get; set; }
        public News News { get; set; }
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
