using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.UOW.DAL.Abstract;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class NewsUrlRepository : Repository<NewsUrl>
    {
        public NewsUrlRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
