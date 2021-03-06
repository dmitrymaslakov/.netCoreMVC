﻿using NewsGatheringService.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace NewsGatheringService.DAL.Entities
{
    public class Role : IEntity
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
