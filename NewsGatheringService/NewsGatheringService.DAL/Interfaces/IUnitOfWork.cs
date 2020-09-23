using NewsGatheringService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsGatheringService.DAL.Interfaces
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
        IRepository<RefreshToken> RefreshTokenRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
