using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringService.Domain.Concrete.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
