using MediatR;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using NewsGatheringServiceWebAPI.Queries;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class GetNewsHandler : IRequestHandler<GetNewsQuery, IQueryable<News>>
    {
        private readonly IRepository<News> _newsRepository;


        public GetNewsHandler(IRepository<News> newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public Task<IQueryable<News>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            var News = _newsRepository
                .FindBy(n => n is News, n => n.Category, n => n.Subcategory, n => n.NewsStructure)
                .Where(n => string.IsNullOrEmpty(request.CategoryName)
                ? (string.IsNullOrEmpty(request.SubcategoryName) || n.Subcategory != null && n.Subcategory.Name.Contains(request.SubcategoryName))
                : n.Category.Name.Contains(request.CategoryName));
            return Task.FromResult(News);
        }

    }
}
