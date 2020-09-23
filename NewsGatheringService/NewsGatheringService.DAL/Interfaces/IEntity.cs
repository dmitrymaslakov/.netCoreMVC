using System;
using System.Collections.Generic;
using System.Text;

namespace NewsGatheringService.DAL.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
