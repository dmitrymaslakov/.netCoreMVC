using NewsGatheringService.DAL.Entities;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Interfaces
{
    /// <summary>
    /// Class for parsing news
    /// </summary>
    public interface INewsParser
    {
        /// <summary>
        /// Parse news at the url
        /// </summary>
        /// <param name="newsUrl"></param>
        /// <returns></returns>
        News Parse(string newsUrl);
    }
}
