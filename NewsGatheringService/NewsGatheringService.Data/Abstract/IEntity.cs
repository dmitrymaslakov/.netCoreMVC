using System;
using System.Collections.Generic;
using System.Text;

namespace NewsGatheringService.Data.Abstract
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
