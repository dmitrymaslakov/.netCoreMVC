using NewsGatheringService.UOW.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class NewsStructureRepository : Repository<NewsStructure>
    {
        public NewsStructureRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
