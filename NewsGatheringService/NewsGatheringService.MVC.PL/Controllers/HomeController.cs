using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using NewsGatheringService.MVC.PL.Models;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.UOW.DAL.Interfaces;
using NewsCollector.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace NewsGatheringService.MVC.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryable<News> _news;
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly int _totalPages;
        const int PAGE_SIZE = 4;
        const int FIRST_PAGE = 1;


        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger, INewsService newsService)
        {
            _unitOfWork = unitOfWork;
            _news = _unitOfWork.NewsRepository
                .FindBy(null, n => n.Category, n => n.Subcategory, n => n.NewsStructure, n => n.Source);
            _logger = logger;
            _newsService = newsService;
            _totalPages = _news.Count() % PAGE_SIZE != 0 ? _news.Count() / PAGE_SIZE + 1 : _news.Count() / PAGE_SIZE;
        }

        //[Authorize(Roles = "admin, user")]
        public IActionResult Index(int? page, string categoryName = null, string subcategoryName = null, int? rate = null)
        {
            try
            {
                //return RedirectToAction("Index", "Admin");
                //return RedirectToAction("DeleteNews", "NewsParser");
                //return RedirectToAction("DeleteUsers", "NewsParser");
                //return RedirectToAction("DeleteRoles", "NewsParser");
                /*var m = new string[] { "string" };
                return RedirectToAction("DeleteUserByLogin", "NewsParser", new { logins = m});*/

                if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
                    return PartialView("_IndexItemsPart", GetNewsModelConfigure(categoryName, subcategoryName, rate, page));

                var newsConfigire = GetNewsModelConfigure(categoryName, subcategoryName, rate, FIRST_PAGE);

                return View(newsConfigire);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest();
            }
        }
        private NewsModelConfigure GetNewsModelConfigure(string categoryName, string subcategoryName, int? rate, int? page = 1)
        {
            try
            {
                var newsConfigire = new NewsModelConfigure(_news)
                {
                    CategoryName = categoryName,
                    SubcategoryName = subcategoryName,
                    ReputationValue = rate
                };

                newsConfigire.RecentFirst();

                newsConfigire.UseFilterBasedOnProp();

                newsConfigire.TotalPages = _totalPages;

                newsConfigire.ItemsPerPage(PAGE_SIZE, (int)page);

                return newsConfigire;
            }
            catch
            {
                throw;
            }
        }
        public PartialViewResult IndexItemsPart(int page, string categoryName = null, string subcategoryName = null, int? rate = null)
        {
            try
            {
                var newsConfigire = new NewsModelConfigure(_news)
                {
                    CategoryName = categoryName,
                    SubcategoryName = subcategoryName,
                    ReputationValue = rate
                };

                newsConfigire.RecentFirst();

                newsConfigire.UseFilterBasedOnProp();

                newsConfigire.TotalPages = _totalPages;

                newsConfigire.ItemsPerPage(PAGE_SIZE, page);

                return PartialView("_IndexItemsPart", newsConfigire);
            }
            catch
            {
                throw;
            }
        }
    }
}
