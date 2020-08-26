using NewsGatheringService.Data.Entities;

namespace NewsCollector.Abstract
{
    public interface INewsParser
    {
        News Parse(string newsUrl);
    }
}
