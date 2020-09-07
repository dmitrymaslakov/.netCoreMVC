using Microsoft.EntityFrameworkCore;
using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringService.Domain.Concrete.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>
    {
        public RefreshTokenRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
