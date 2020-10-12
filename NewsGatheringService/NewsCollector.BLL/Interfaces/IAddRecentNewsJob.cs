using System.Threading.Tasks;

namespace NewsCollector.BLL.Interfaces
{

    public interface IAddRecentNewsJob
    {
        Task AddUrlsNewsToDb();
    }
}
