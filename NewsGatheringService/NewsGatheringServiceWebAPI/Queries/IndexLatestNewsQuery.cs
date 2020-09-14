using MediatR;
using NewsGatheringService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class IndexLatestNewsQuery : IRequest<IEnumerable<RecentNews>>
    {
    }
}
