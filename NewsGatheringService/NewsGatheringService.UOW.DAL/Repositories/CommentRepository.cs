using NewsGatheringService.UOW.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
