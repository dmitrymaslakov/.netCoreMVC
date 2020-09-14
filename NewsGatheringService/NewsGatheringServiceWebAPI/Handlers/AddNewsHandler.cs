using MediatR;
using NewsCollector.Abstract;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using NewsGatheringServiceWebAPI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class AddNewsHandler : IRequestHandler<AddNewsCommand, bool>
    {
        private readonly INewsService _newsService;

        public AddNewsHandler(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<bool> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newsForDb = _newsService.GetNewsDataFromRss().Join(request.LatestNewsUrls,
                    nd => nd.Id,
                    nUrl => nUrl,
                    (nd, nUrl) => nd);

                if (newsForDb.Count() != 0)
                {
                    await _newsService.InsertNewsIntoDb(newsForDb.ToArray());
                    return true;
                }
                else return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
