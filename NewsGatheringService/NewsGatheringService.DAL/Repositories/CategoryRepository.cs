using NewsGatheringService.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
