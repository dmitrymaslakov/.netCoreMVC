using MediatR;
using NewsGatheringService.DAL.Entities;
using System.Linq;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class GetSubcategoriesQuery : IRequest<IQueryable<Subcategory>>
    {
        public GetSubcategoriesQuery(string categoryName = null)
        {
            if (!string.IsNullOrEmpty(categoryName))
                CategoryName = categoryName;
        }
       
        public string CategoryName { get; set; }
    }
}