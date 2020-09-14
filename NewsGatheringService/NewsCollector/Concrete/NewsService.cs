using NewsCollector.Abstract;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NewsCollector.Concrete
{
    public class NewsService : INewsService
    {

        private readonly IRssReader _rssReader;
        private readonly IOnlinerNewsParser _onlinerNewsParser;
        private readonly Is13NewsParser _s13NewsParser;
        private readonly ITutByNewsParser _tutByNewsParser;
        private readonly IUnitOfWork _unitOfWork;

        const string s13 = "s13.ru";
        const string onliner = "onliner.by";
        const string tut = "tut.by";

        public NewsService(
            IRssReader rssReader,
            IOnlinerNewsParser onlinerNewsParser,
            Is13NewsParser s13NewsParser,
            IUnitOfWork unitOfWork, ITutByNewsParser tutByNewsParser)
        {
            _rssReader = rssReader;
            _onlinerNewsParser = onlinerNewsParser;
            _s13NewsParser = s13NewsParser;
            _unitOfWork = unitOfWork;
            _tutByNewsParser = tutByNewsParser;
        }

        public async Task InsertNewsIntoDb(SyndicationItem[] newsData)
        {
            var newsSet = new List<News>();
            try
            {
                var newsDb = //await 
                    _unitOfWork.NewsRepository.GetAllAsync();
                var i = 1;
                foreach (var newsItem in newsData)
                {
                    if (newsDb.Any(n => n.Source.Equals(newsItem)))
                        continue;
                    News news = null;
                    if (newsItem.Id.Contains(onliner))
                        news = _onlinerNewsParser.Parse(newsItem.Id);
                    else if (newsItem.Id.Contains(s13))
                        news = _s13NewsParser.Parse(newsItem.Id);
                    else if (newsItem.Id.Contains(tut))
                        news = _tutByNewsParser.Parse(newsItem.Id);

                    newsSet.Add(news);
                    i++;
                }

                await _unitOfWork.CategoryRepository.AddRangeAsync(newsSet
                    .GroupBy(n => n.Category.Name)
                    .Select(gr => gr.First().Category)
                    .Where(c =>
                    {
                        var cRep = _unitOfWork.CategoryRepository.GetAllAsync();//.Result;
                        return !cRep.Any() || !cRep.Select(c => c.Name).Any(c2 => c2.Equals(c.Name));
                    }));

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.SubcategoryRepository.AddRangeAsync(newsSet
                    .Where(n => !string.IsNullOrEmpty(n.Subcategory.Name))
                    .Where(n =>
                    {
                        var scRep = _unitOfWork.SubcategoryRepository.GetAllAsync();//.Result;
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

                await _unitOfWork.NewsRepository.AddRangeAsync(newsSet.Select(n =>
                {
                    n.Category = _unitOfWork.CategoryRepository.FindBy(c => c.Name.Equals(n.Category.Name)).FirstOrDefault();
                    n.Subcategory = _unitOfWork.SubcategoryRepository.FindBy(s => s.Name.Equals(n.Subcategory.Name)).FirstOrDefault();
                    return n;
                }));
                await _unitOfWork.SaveChangesAsync();

            }
            catch
            {
                throw;
            }
        }
        public SyndicationItem[] GetNewsDataFromRss()
        {
            var newsDataFromRss = new ConcurrentBag<SyndicationItem>();
            try
            {
                Parallel.ForEach(SourceStorage.RssFeeds, parallelOptions: new ParallelOptions() { MaxDegreeOfParallelism = 6 },
                    s =>
                    {
                        var items = _rssReader.GetNewsDataFromRssFeed(s);
                        Parallel.ForEach(items, item =>
                        {
                            if (item.Id.Contains(s13) || item.Id.Contains(onliner) || item.Id.Contains(tut))
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
    }
}
