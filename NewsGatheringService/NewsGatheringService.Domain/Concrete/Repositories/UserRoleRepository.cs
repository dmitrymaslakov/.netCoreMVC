using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringService.Domain.Concrete.Repositories
{
    public class UserRoleRepository : Repository<UserRole>
    {
        public UserRoleRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
