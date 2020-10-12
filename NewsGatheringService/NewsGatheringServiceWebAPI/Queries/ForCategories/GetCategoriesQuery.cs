using MediatR;
using NewsGatheringService.DAL.Entities;
using System;
using System.Linq;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class GetCategoriesQuery : IRequest<IQueryable<Category>>
    {

    }
}