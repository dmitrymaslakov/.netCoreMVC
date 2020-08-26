using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringService.Domain.Concrete.Repositories
{
    public class SubcategoryRepository : Repository<Subcategory>
    {
        public SubcategoryRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
