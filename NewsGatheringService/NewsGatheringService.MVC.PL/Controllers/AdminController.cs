using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.MVC.PL.Models;
using Microsoft.Extensions.Logging;
using NewsGatheringService.DAL.Interfaces;
using NewsGatheringService.BLL.Interfaces;

namespace NewsGatheringService.MVC.PL.Controllers
{
    public class AdminController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newsService;
        private readonly ILogger<AdminController> _logger;
        //private readonly List<SyndicationItem> recentDataNews = new List<SyndicationItem>();

        public AdminController(IUnitOfWork unitOfWork, INewsService newsService, ILogger<AdminController> logger)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
            _logger = logger;

        }
        /// <summary>
        /// Displays an index of the latest news
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var recentDataNews = _newsService.GetRecentNewsDataFromRss();

                /*var newsDb = _unitOfWork.NewsRepository.GetAllAsync();

                foreach (var syndicationItem in _newsService.GetNewsDataFromRss())
                {
                    if (newsDb.Any(n => n.Source.Equals(syndicationItem.Id)))
                        continue;
                    recentDataNews.Add(syndicationItem);
                }*/

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
                await _newsService.AddRecentNewsToDbAsync(newsUrls);
                /*var newsForDb = _newsService.GetNewsDataFromRss().Join(newsUrls,
                    nd => nd.Id,
                    nUrl => nUrl,
                    (nd, nUrl) => nd);

                if (newsForDb.Count() != 0)
                {
                    await _newsService.InsertNewsIntoDb(newsForDb.ToArray());
                }*/
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
