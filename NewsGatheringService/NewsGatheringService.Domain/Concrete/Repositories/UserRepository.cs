using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringService.Domain.Concrete.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
