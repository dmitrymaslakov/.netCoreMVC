using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringService.Domain.Concrete.Repositories
{
    public class NewsStructureRepository : Repository<NewsStructure>
    {
        public NewsStructureRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
