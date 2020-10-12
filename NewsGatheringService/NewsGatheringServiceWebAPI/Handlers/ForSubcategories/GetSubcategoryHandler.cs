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
    public class GetSubcategoryHandler : IRequestHandler<GetSubcategoryQuery, Subcategory>
    {
        private readonly IRepository<Subcategory> _subcategoryRepository;

        public GetSubcategoryHandler(IRepository<Subcategory> subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        public Task<Subcategory> Handle(GetSubcategoryQuery request, CancellationToken cancellationToken)
        {
            var Subcategory = _subcategoryRepository
                .FindBy(sc => sc.Name.Equals(request.Name))
                .FirstOrDefault();
            
            return Task.FromResult(Subcategory);
        }
    }
}
