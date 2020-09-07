using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceMVC.Models
{
    public class Menu
    {
        IEnumerable<string> Category { get; set; }
        /*IEnumerable<string> Subcategory { get; set; }*/
    }
}
