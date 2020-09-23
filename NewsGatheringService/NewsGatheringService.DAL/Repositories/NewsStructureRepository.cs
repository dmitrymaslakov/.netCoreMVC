using NewsGatheringService.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.DAL.Repositories
{
    public class NewsStructureRepository : Repository<NewsStructure>
    {
        public NewsStructureRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
