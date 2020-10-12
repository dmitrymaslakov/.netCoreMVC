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
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IQueryable<Category>>
    {
        private readonly IRepository<Category> _categoryRepository;

        public GetCategoriesHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<IQueryable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var Categories = _categoryRepository
                .FindBy(c => c is Category);
            
            return Task.FromResult(Categories);
        }
    }
}
