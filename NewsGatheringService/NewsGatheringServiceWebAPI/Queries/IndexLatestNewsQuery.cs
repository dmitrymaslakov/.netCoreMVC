using MediatR;
using NewsGatheringService.Models.BLL;
using System.Collections.Generic;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class IndexLatestNewsQuery : IRequest<IEnumerable<RecentNews>>
    {
    }
}
