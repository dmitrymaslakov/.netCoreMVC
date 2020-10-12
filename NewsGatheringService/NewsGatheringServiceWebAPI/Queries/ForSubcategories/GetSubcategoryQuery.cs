using MediatR;
using NewsGatheringService.DAL.Entities;
using System;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class GetSubcategoryQuery : IRequest<Subcategory>
    {
        public GetSubcategoryQuery(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}