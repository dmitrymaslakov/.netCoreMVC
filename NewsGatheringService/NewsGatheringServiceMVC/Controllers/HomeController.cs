using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using NewsCollector.Abstract;
using NewsGatheringServiceMVC.Models;
using Microsoft.AspNetCore.Authorization;
using NewsGatheringService.Data.Abstract;

namespace NewsGatheringServiceMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryable<News> _news;
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly int _totalPages;
        const int pageSize = 5;
        const int firstPage = 1;


        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger, INewsService newsService)
        {
            _unitOfWork = unitOfWork;
            _news = _unitOfWork.NewsRepository.FindBy(n => n is News, n => n.Category, n => n.Subcategory, n => n.NewsStructure);
            _logger = logger;
            _newsService = newsService;
            _totalPages = _news.Count() % pageSize != 0 ? _news.Count() / pageSize + 1 : _news.Count() / pageSize;
        }

        //[Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index(string categoryName = null, string subcategoryName = null)
        {
            try
            {
                //return RedirectToAction("Index", "Admin");
                //return RedirectToAction("Index", "NewsParser");
                if (!_news.Any())
                    await _newsService.InsertNewsIntoDb(_newsService.GetNewsDataFromRss());

                var newsConfigire = new NewsModelConfigure(_news)
                {
                    CategoryName = categoryName,
                    SubcategoryName = subcategoryName
                };
                newsConfigire.UseFilterCategory();
                newsConfigire.TotalPages = _totalPages;
                newsConfigire.ItemsPerPage(pageSize, firstPage);
                newsConfigire.RecentFirst();
                return View(newsConfigire);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        public PartialViewResult IndexItemsPart(int page, string categoryName = null, string subcategoryName = null)
        {
            /*var arr = newsids.Split(' ');
            var newsIds = arr.Select(e => new Guid(e));*/

            var newsConfigire = new NewsModelConfigure(_news)
            {
                CategoryName = categoryName,
                SubcategoryName = subcategoryName
            };
            newsConfigire.UseFilterCategory();
            newsConfigire.TotalPages = _totalPages;
            newsConfigire.ItemsPerPage(pageSize, page);
            newsConfigire.RecentFirst();
            return PartialView("_IndexItemsPart", newsConfigire);
        }
        public IActionResult IndexDependingOnCategory(string categoryName, string subcategoryName)
        {
            try
            {
                var newsResult = string.IsNullOrEmpty(categoryName)
                    ? _news.Where(n => n.Subcategory != null && n.Subcategory.Name.Contains(subcategoryName))
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
