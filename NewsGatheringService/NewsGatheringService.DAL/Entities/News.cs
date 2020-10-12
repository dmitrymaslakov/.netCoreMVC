using NewsGatheringService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsGatheringService.DAL.Entities
{
    public class News : IEntity
    {
        public News()
        {
            Comments = new HashSet<Comment>();
        }

        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubcategoryId { get; set; }
        public DateTime Date { get; set; }
        public NewsUrl Source { get; set; }
        public string Author { get; set; }
        public byte[] NewsHeaderImage { get; set; }
        public int Reputation { get; set; }
        public NewsStructure NewsStructure { get; set; }
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
