using NewsGatheringService.UOW.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class UserRoleRepository : Repository<UserRole>
    {
        public UserRoleRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
