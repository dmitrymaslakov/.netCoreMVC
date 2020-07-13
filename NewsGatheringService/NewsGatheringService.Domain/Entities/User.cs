using System;
using System.Collections.Generic;

namespace NewsGatheringService.Domain
{
    public class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public byte[] ProfilePicture { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
