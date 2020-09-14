using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Models
{
    public class NewsDTO
    {
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public string Author { get; set; }
        public byte[] NewsHeaderImage { get; set; }
        public int Reputation { get; set; }

        public NewsStructureDTO NewsStructure { get; set; }
        //public IEnumerable<NewsCategory> NewsCategories { get; set; }
        //public NewsCategory NewsCategory { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }

    }
}
