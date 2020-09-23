﻿using NewsGatheringService.DAL.Abstract;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}