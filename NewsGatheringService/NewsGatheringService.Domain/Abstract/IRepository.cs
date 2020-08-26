using NewsGatheringService.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsGatheringService.Domain.Abstract
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; set; }
    }
}
