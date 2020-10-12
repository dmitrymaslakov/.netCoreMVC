using MediatR;
using NewsGatheringService.DAL.Entities;
using System;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class GetOneNewsQuery : IRequest<News>
    {
        public GetOneNewsQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}