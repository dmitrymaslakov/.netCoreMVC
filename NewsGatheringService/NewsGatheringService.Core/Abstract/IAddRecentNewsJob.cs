using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsGatheringService.Core.Abstract
{
    public interface IAddRecentNewsJob
    {
        Task AddNews();
    }
}
