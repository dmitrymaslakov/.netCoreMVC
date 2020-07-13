using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceMVC.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IRepository _db;
        public MenuViewComponent(IRepository db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            return View("Menu", _db.Categories);
        }
    }
}
