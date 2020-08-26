using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using NewsCollector.Abstract;

namespace NewsGatheringServiceMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnumerable<News> _news;
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;


        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger, INewsService newsService)
        {
            _unitOfWork = unitOfWork;
            _news = _unitOfWork.NewsRepository.FindBy(n => n is News, n => n.Category, n => n.Subcategory, n => n.NewsStructure);
            _logger = logger;
            _newsService = newsService;
        }

        //[Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            try
            {
                //return RedirectToAction("Index", "Admin");
                //return RedirectToAction("Index", "NewsParser");
                //_newsService.AttemptedToDivideByZero();
                if (!_news.Any())
                    await _newsService.GetDataFromRssAndInsertIntoDb();

                return View(_news.OrderByDescending(n => n.Date));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation(ex.Message);
                _logger.LogWarning(ex.Message);
                _logger.LogCritical(ex.Message);                
                return BadRequest();
            }
        }
        public IActionResult IndexDependingOnCategory(string categoryName, string subcategoryName)
        {
            try
            {
                var newsResult = string.IsNullOrEmpty(categoryName) 
                    ? _news.Where(n => n.Subcategory!=null && n.Subcategory.Name.Contains(subcategoryName))
                    : _news.Where(n => n.Category != null && n.Category.Name.Contains(categoryName));
                return View("Index", newsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
    }
}
