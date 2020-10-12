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
    public class GetOneNewsHandler : IRequestHandler<GetOneNewsQuery, News>
    {
        private readonly IRepository<News> _newsRepository;

        public GetOneNewsHandler(IRepository<News> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public Task<News> Handle(GetOneNewsQuery request, CancellationToken cancellationToken)
        {
            var News = _newsRepository
                .FindBy(n => n.Id.CompareTo(request.Id) == 0, n => n.Category, n => n.Subcategory, n => n.NewsStructure)
                .FirstOrDefault();
            
            return Task.FromResult(News);
        }
    }
}
