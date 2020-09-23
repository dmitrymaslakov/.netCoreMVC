using NewsGatheringService.DAL.Entities;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Interfaces
{
    public interface INewsParser
    {
        Task<News> ParseAsync(string newsUrl);
    }
}
