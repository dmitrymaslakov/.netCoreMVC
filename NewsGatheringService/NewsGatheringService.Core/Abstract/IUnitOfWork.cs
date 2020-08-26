using NewsGatheringService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsGatheringService.Core.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<Category> CategoryRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<News> NewsRepository { get; }
        IRepository<NewsStructure> NewsStructureRepository { get; }
        IRepository<Role> RoleRepository { get; }
        IRepository<Subcategory> SubcategoryRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<UserRole> UserRoleRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
