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
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, Category>
    {
        private readonly IRepository<Category> _categoryRepository;

        public GetCategoryHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<Category> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var News = _categoryRepository
                .FindBy(c => c.Name.Equals(request.CategoryName))
                .FirstOrDefault();
            
            return Task.FromResult(News);
        }
    }
}
