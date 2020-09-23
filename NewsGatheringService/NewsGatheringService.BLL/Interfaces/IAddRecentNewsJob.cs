using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsGatheringService.BLL.Interfaces
{
    public interface IAddRecentNewsJob
    {
        Task AddNews();
    }
}
