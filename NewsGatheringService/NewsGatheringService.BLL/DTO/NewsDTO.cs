using System;
using System.Collections.Generic;
using System.Linq;
namespace NewsGatheringService.BLL.DTO
{
    public class NewsDTO
    {
        public DateTime Date { get; set; }
        public NewsUrlDTO Source { get; set; }
        public string Author { get; set; }
        public string NewsHeaderImage { get; set; }
        public int Reputation { get; set; }
        public NewsStructureDTO NewsStructure { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }

    }
}
