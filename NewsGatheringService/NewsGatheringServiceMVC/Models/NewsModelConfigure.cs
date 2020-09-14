using NewsGatheringService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceMVC.Models
{
    public class NewsModelConfigure
    {
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public int TotalPages { get; set; }
        public IQueryable<News> News { get; set; }
        public NewsModelConfigure(IQueryable<News> news)
        {
            News = news;
        }

        public void RecentFirst()
        {
            News.OrderByDescending(n => n.Date);
        }
        public void OldFirst()
        {
            News.OrderBy(n => n.Date);
        }
        public void UseFilterCategory()
        {
            News = News
                .Where(n =>
                string.IsNullOrEmpty(CategoryName)
                ? (string.IsNullOrEmpty(SubcategoryName)
                    || n.Subcategory != null && n.Subcategory.Name.Contains(SubcategoryName))
                : n.Category.Name.Contains(CategoryName));
        }
        public void ItemsPerPage(int pageSize, int numPage)
        {
            News = News
                .Skip((numPage - 1) * pageSize)
                .Take(pageSize);
        }
    }

}
