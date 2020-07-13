﻿using System;
using System.Collections.Generic;

namespace NewsGatheringService.Domain
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime Date { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
