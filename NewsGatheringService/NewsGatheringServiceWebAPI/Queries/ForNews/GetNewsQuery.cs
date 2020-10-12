using MediatR;
using NewsGatheringService.DAL.Entities;
using System.Linq;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class GetNewsQuery : IRequest<IQueryable<News>>
    {
        public GetNewsQuery(int? rate = null, string categoryName = null, string subcategoryName = null)
        {
            if (rate != null)
                Rate = rate;
            
            if (!string.IsNullOrEmpty(categoryName))
                CategoryName = categoryName;
            
            if (!string.IsNullOrEmpty(subcategoryName))
                SubcategoryName = subcategoryName;

        }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public int? Rate { get; set; }
    }
}