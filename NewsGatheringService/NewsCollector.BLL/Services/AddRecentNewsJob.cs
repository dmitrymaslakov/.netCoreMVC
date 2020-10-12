using NewsCollector.BLL.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Services
{
    public class AddRecentNewsJob : IAddRecentNewsJob
    {
        private readonly INewsService _newsService;
        public AddRecentNewsJob(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task AddUrlsNewsToDb()
        {
            try
            {
                await _newsService.InsertNewsUrlsToDb(_newsService.GetNewsData().Select(nd => nd.Id).ToArray());
            }
            catch
            {
                throw;
            }
        }
    }
}
