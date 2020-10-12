using NewsGatheringService.UOW.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
