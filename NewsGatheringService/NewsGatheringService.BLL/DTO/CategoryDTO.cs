using NewsGatheringService.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace NewsGatheringService.BLL.DTO
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
