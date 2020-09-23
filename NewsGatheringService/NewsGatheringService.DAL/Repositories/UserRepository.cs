using NewsGatheringService.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.DAL.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
