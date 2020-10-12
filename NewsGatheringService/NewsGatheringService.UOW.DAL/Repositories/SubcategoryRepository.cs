using NewsGatheringService.UOW.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class SubcategoryRepository : Repository<Subcategory>
    {
        public SubcategoryRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
