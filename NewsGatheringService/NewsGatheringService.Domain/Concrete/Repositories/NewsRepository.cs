using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringService.Domain.Concrete.Repositories
{
    public class NewsRepository : Repository<News>
    {
        public NewsRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
