using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringService.Domain;
using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringServiceMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _db;

        public HomeController(IRepository db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.News);
        }
    }
}
