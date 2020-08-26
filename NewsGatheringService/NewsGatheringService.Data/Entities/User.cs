using NewsGatheringService.Data.Abstract;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NewsGatheringService.Data.Entities
{
    public class User : IEntity
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
