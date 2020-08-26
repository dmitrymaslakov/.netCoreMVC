using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringService.Domain.Concrete.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
