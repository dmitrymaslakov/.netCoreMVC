using MediatR;
using NewsGatheringService.DAL.Entities;
using System.Linq;

namespace NewsGatheringServiceWebAPI.Queries
{
    public class GetNewsQuery : IRequest<IQueryable<News>>
    {
        public GetNewsQuery(string categoryName = null, string subcategoryName = null)
        {
            if (!string.IsNullOrEmpty(categoryName))
                CategoryName = categoryName;
            if (!string.IsNullOrEmpty(subcategoryName))
                SubcategoryName = subcategoryName;

        }
        /*public GetNewsQuery(bool returnFirst)
        {
            ReturnFirst = returnFirst;
        }
        public bool ReturnFirst { get; set; }*/
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
    }
}