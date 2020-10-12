using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using NewsCollector.BLL.Interfaces;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.DAL.Models;
using Microsoft.Extensions.Options;
using NewsGatheringService.UOW.DAL.Interfaces;
using System;

namespace NewsCollector.BLL.Services
{
    public class NewsService : INewsService
    {

        private readonly IRssReader _rssReader;
        private readonly IOnlinerNewsParser _onlinerNewsParser;
        private readonly Is13NewsParser _s13NewsParser;
        private readonly ITutByNewsParser _tutByNewsParser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        private readonly INewsEvaluation _newsEvaluation;

        const string S13 = "s13.ru";
        const string ONLINER = "onliner.by";
        const string TUT = "tut.by";
        const string HELPBLOG = "help.blog";
        const string SPECIALTUT = "special.tut";

        public NewsService(
            IRssReader rssReader,
            IOnlinerNewsParser onlinerNewsParser,
            Is13NewsParser s13NewsParser,
            IUnitOfWork unitOfWork,
            ITutByNewsParser tutByNewsParser,
            IOptions<AppSettings> appSettings,
            INewsEvaluation newsEvaluation)
        {
            _rssReader = rssReader;
            _onlinerNewsParser = onlinerNewsParser;
            _s13NewsParser = s13NewsParser;
            _unitOfWork = unitOfWork;
            _tutByNewsParser = tutByNewsParser;
            _appSettings = appSettings.Value;
            _newsEvaluation = newsEvaluation;
        }

        public async Task ParseNewsAndInsertIntoDb()
        {
            var newsSet = new ConcurrentBag<News>();

            try
            {
                var dbNewsUrls = _unitOfWork.NewsUrlRepository
                    .FindBy(null, nu => nu.News)
                    .Where(nu => nu.News == null)
                    .Select(nu => nu.Url)
                    .ToArray();

                if (!dbNewsUrls.Any()) return;

                Parallel.ForEach(dbNewsUrls, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, newsUrl =>
                {
                    News news = null;

                    if (newsUrl.Contains(ONLINER))
                        news = _onlinerNewsParser.Parse(newsUrl);

                    else if (newsUrl.Contains(S13))
                        news = _s13NewsParser.Parse(newsUrl);

                    else if (newsUrl.Contains(TUT) && !newsUrl.Contains(HELPBLOG) && !newsUrl.Contains(SPECIALTUT))
                    {
                        news = _tutByNewsParser.Parse(newsUrl);
                    }

                    if (news != null)
                        newsSet.Add(news);
                });

                await AddCategoryAndSubcategoryToDb(newsSet.ToArray());

                await _unitOfWork.NewsRepository.AddRangeAsync(newsSet
                    .ToArray()
                    .Select(n =>
                    {
                        n.Category = _unitOfWork.CategoryRepository.FindBy(c => c.Name.Equals(n.Category.Name)).FirstOrDefault();
                        n.Subcategory = _unitOfWork.SubcategoryRepository.FindBy(s => s.Name.Equals(n.Subcategory.Name)).FirstOrDefault();
                        n.Source = _unitOfWork.NewsUrlRepository.FindBy(url => url.Url.Equals(n.Source.Url)).FirstOrDefault();
                        return n;
                    }));

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task InsertNewsUrlsToDb(string[] newsUrls)
        {
            try
            {
                await _unitOfWork.NewsUrlRepository.AddRangeAsync(
                    newsUrls.Select(url => new NewsUrl { Id = Guid.NewGuid(), Url = url })
                    );
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public SyndicationItem[] GetNewsData()
        {
            var newsDataFromRss = new ConcurrentBag<SyndicationItem>();

            try
            {
                var dbNewsUrl = _unitOfWork.NewsUrlRepository.GetAllAsQueryable().ToArray();

                Parallel.ForEach(_appSettings.RssFeeds, new ParallelOptions() { MaxDegreeOfParallelism = 3 },
                    s =>
                    {
                        var items = _rssReader.GetNewsDataFromRssFeed(s);

                        Parallel.ForEach(items, item =>
                        {
                            if (item.Id.Contains(S13) || item.Id.Contains(ONLINER) || item.Id.Contains(TUT))
                                if (dbNewsUrl.Count() == 0 || !dbNewsUrl.Any(nu => nu.Url.Equals(item.Id)))
                                    newsDataFromRss.Add(item);
                        });
                    });

                return newsDataFromRss.ToArray();
            }
            catch
            {
                throw;
            }
        }

        public async Task PerformNewsEvaluationAsync()
        {
            try
            {
                var unratedNewsSet = _unitOfWork.NewsRepository
                    .FindBy(null, n => n.NewsStructure)
                    .Where(n => n.Reputation == 0)
                    .ToArray();

                if (unratedNewsSet.Count() == 0) return;

                var updatedNewSet = new ConcurrentBag<News>();

                Parallel.ForEach(unratedNewsSet, async n =>
                {
                    n.Reputation = await _newsEvaluation.EvaluateNewsAsync(n);
                    updatedNewSet.Add(n);
                });

                _unitOfWork.NewsRepository.UpdateRange(updatedNewSet.ToArray());

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        private async Task AddCategoryAndSubcategoryToDb(News[] newsSet)
        {
            try
            {
                await _unitOfWork.CategoryRepository.AddRangeAsync(newsSet
                    .GroupBy(n =>
                    {
                        if (n == null)
                        {
                            var f = newsSet;
                        }
                        return n.Category.Name;
                    })
                    .Select(gr => gr.First().Category)
                    .Where(c =>
                    {
                        var cRep = _unitOfWork.CategoryRepository.GetAllAsQueryable();
                        return !cRep.Any() || !cRep.Select(c => c.Name).Any(c2 => c2.Equals(c.Name));
                    }));

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.SubcategoryRepository.AddRangeAsync(newsSet
                    .Where(n => !string.IsNullOrEmpty(n.Subcategory.Name))
                    .Where(n =>
                    {
                        var scRep = _unitOfWork.SubcategoryRepository.GetAllAsQueryable();
                        if (scRep.Any())
                        {
                            var b = scRep.Select(sc => sc.Name).Any(cs2 => cs2.Equals(n.Subcategory.Name));
                        }
                        return !scRep.Any() || !scRep.Select(sc => sc.Name).Any(cs2 => cs2.Equals(n.Subcategory.Name));
                    })
                    .GroupBy(n => n.Subcategory.Name)
                    .Select(gr =>
                    {
                        var n = gr.First();
                        n.Subcategory.Category = _unitOfWork.CategoryRepository
                        .FindBy(c => c.Name.Equals(n.Category.Name)).FirstOrDefault();
                        return n.Subcategory;
                    }));

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
