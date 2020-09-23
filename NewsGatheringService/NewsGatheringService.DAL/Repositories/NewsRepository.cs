using NewsGatheringService.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.DAL.Repositories
{
    public class NewsRepository : Repository<News>
    {
        public NewsRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
