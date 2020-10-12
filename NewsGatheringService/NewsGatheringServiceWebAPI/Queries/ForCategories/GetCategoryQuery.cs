using MediatR;
using NewsGatheringService.DAL.Entities;
using System;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class GetCategoryQuery : IRequest<Category>
    {
        public GetCategoryQuery(string categoryName)
        {
            CategoryName = categoryName;
        }
        public string CategoryName { get; set; }
    }
}