using MediatR;
using NewsGatheringService.DAL.Entities;
using System;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}