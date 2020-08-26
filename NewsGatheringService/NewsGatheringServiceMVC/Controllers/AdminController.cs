using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsCollector.Abstract;
using NewsGatheringService.Core.Abstract;
using NewsGatheringServiceMVC.Models;
using Microsoft.Extensions.Logging;


namespace NewsGatheringServiceMVC.Controllers
{
    public class AdminController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newsService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUnitOfWork unitOfWork, INewsService newsService, ILogger<AdminController> logger)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var rssData = new List<SyndicationItem>();
                var newsDb = await _unitOfWork.NewsRepository.GetAllAsync();

                foreach (var syndicationItem in _newsService.GetDataFromRss())
                {
                    if (newsDb.Any(n => n.Source.Equals(syndicationItem.Id)))
                        continue;
                    rssData.Add(syndicationItem);
                }


                var recentNews = rssData.Select(i => new RecentNews(i));
                return View(recentNews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddNewsToDb(string[] newsIds)
        {
            try
            {
                await _newsService.GetDataFromRssAndInsertIntoDb();
                TempData["message"] = "Новости успешно сохранены";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
