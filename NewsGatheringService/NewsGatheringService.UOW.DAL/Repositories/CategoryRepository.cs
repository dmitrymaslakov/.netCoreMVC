using NewsGatheringService.UOW.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
