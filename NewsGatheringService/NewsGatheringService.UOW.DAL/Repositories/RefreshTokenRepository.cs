using NewsGatheringService.UOW.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>
    {
        public RefreshTokenRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
