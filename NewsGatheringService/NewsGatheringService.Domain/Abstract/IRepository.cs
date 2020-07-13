using System;
using System.Collections.Generic;
using System.Text;

namespace NewsGatheringService.Domain.Abstract
{
    public interface IRepository
    {
        IEnumerable<Category> Categories { get; }
        IEnumerable<Comment> Comments { get; }
        IEnumerable<News> News { get; }
        IEnumerable<NewsStructure> NewsStructure { get; }
        IEnumerable<Role> Roles { get; }
        IEnumerable<Subcategory> Subcategories { get; }
        IEnumerable<UserRole> UserRoles { get; }
        IEnumerable<User> Users { get; }
        void SaveMerch(object entity);
        object DeleteMerch(int entityId);
    }
}
