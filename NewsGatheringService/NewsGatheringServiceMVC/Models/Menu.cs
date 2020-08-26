using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceMVC.Models
{
    public class Menu
    {
        private readonly IRepository _db;
        public Menu(IRepository db)
        {
            /*_db = db;
            var groupCategory = db.Categories
                .GroupBy(c => c.Name);*/
        }

        IEnumerable<string> Category { get; set; }
        IEnumerable<string> Subcategory { get; set; }
    }
}
