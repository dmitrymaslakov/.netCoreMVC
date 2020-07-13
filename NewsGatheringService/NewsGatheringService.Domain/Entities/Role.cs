using System;
using System.Collections.Generic;

namespace NewsGatheringService.Domain
{
    public class Role
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
