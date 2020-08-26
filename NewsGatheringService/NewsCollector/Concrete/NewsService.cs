using NewsCollector.Abstract;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

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

        /*public void AttemptedToDivideByZero()
        {
            var i = 0;
            var res = 1 / i;
        }*/
        public async Task GetDataFromRssAndInsertIntoDb()
        {
            var newsSet = new List<News>();
            try
            {
                var rssData = GetDataFromRss();
                var newsDb = await _unitOfWork.NewsRepository.GetAllAsync();
                var i = 1;
                foreach (var syndicationItem in rssData)
                {

                    if (newsDb.Any(n => n.Source.Equals(syndicationItem.Id)))
                        continue;              
                    
                    News news = null;
                    if (syndicationItem.Id.Contains(onliner))
                        news = _onlinerNewsParser.Parse(syndicationItem.Id);
                    else if (syndicationItem.Id.Contains(s13))
                        news = _s13NewsParser.Parse(syndicationItem.Id);
                    else if (syndicationItem.Id.Contains(tut))
                        news = _tutByNewsParser.Parse(syndicationItem.Id);

                    newsSet.Add(news);
                    i++;
                }

                await _unitOfWork.CategoryRepository.AddRangeAsync(newsSet
                    .GroupBy(n => n.Category.Name)
                    .Select(gr => gr.First().Category));

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.SubcategoryRepository.AddRangeAsync(newsSet
                    .Where(n => !string.IsNullOrEmpty(n.Subcategory.Name))
                    .Select(n =>
                    {
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
            catch (Exception ex)
            {
                var exMess = ex.Message;
            }
        }

        public SyndicationItem[] GetDataFromRss()
        {
            var newsDataFromRss = new ConcurrentBag<SyndicationItem>();

            Parallel.ForEach(SourceStorage.RssFeeds, parallelOptions: new ParallelOptions() { MaxDegreeOfParallelism = 3 },
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
    }
}
