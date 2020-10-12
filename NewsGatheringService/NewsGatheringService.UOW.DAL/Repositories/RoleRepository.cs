using NewsGatheringService.UOW.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
