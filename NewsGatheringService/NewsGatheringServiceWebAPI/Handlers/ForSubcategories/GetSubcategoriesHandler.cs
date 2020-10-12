using MediatR;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.UOW.DAL.Interfaces;
using NewsGatheringServiceWebAPI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class GetSubcategoriesHandler : IRequestHandler<GetSubcategoriesQuery, IQueryable<Subcategory>>
    {
        private readonly IRepository<Subcategory> _subcategoryRepository;

        public GetSubcategoriesHandler(IRepository<Subcategory> subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        public Task<IQueryable<Subcategory>> Handle(GetSubcategoriesQuery request, CancellationToken cancellationToken)
        {
            var Subcategories = _subcategoryRepository
                .FindBy(sc => sc is Subcategory, sc => sc.Category)
                .Where(sc => string.IsNullOrEmpty(request.CategoryName) || sc.Category.Name.Contains(request.CategoryName))
                ;
            
            return Task.FromResult(Subcategories);
        }
    }
}
