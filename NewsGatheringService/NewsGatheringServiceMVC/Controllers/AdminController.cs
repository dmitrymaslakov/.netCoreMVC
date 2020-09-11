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
        private readonly List<SyndicationItem> recentDataNews = new List<SyndicationItem>();

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
                var newsDb = await _unitOfWork.NewsRepository.GetAllAsync();

                foreach (var syndicationItem in _newsService.GetNewsDataFromRss())
                {
                    if (newsDb.Any(n => n.Source.Equals(syndicationItem.Id)))
                        continue;
                    recentDataNews.Add(syndicationItem);
                }

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
                var newsForDb = _newsService.GetNewsDataFromRss().Join(newsUrls,
                    nd => nd.Id,
                    nUrl => nUrl,
                    (nd, nUrl) => nd);

                if (newsForDb.Count() != 0)
                {
                    await _newsService.InsertNewsIntoDb(newsForDb.ToArray());
                }
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
