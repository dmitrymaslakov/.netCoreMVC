using NewsGatheringService.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.DAL.Repositories
{
    public class SubcategoryRepository : Repository<Subcategory>
    {
        public SubcategoryRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
