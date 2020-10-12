using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.MVC.PL.Models;
using Microsoft.Extensions.Logging;
using NewsGatheringService.UOW.DAL.Interfaces;
using NewsCollector.BLL.Interfaces;

namespace NewsGatheringService.MVC.PL.Controllers
{
    public class AdminController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(INewsService newsService, ILogger<AdminController> logger)
        {
            _newsService = newsService;
            
            _logger = logger;
        }
        /// <summary>
        /// Displays an index of the latest news
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var recentDataNews = _newsService.GetNewsData();

                var recentNews = recentDataNews.Select(i => new RecentNews(i));

                return View(recentNews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewsToDb(string[] newsUrls)
        {
            try
            {
                if (newsUrls.Any())
                    return RedirectToAction("Index");

                await _newsService.InsertNewsUrlsToDb(newsUrls);
                
                await _newsService.ParseNewsAndInsertIntoDb();

                await _newsService.PerformNewsEvaluationAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return BadRequest();
            }
        }
    }
}
