using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.UOW.DAL.Interfaces;
using System.Threading.Tasks;

namespace NewsGatheringService.UOW.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsAggregatorContext _context;

        public IRepository<Category> CategoryRepository { get; }
        public IRepository<Comment> CommentRepository { get; }
        public IRepository<News> NewsRepository { get; }
        public IRepository<NewsStructure> NewsStructureRepository { get; }
        public IRepository<NewsUrl> NewsUrlRepository { get; }
        public IRepository<Role> RoleRepository { get; }
        public IRepository<Subcategory> SubcategoryRepository { get; }
        public IRepository<User> UserRepository { get; }
        public IRepository<UserRole> UserRoleRepository { get; }
        public IRepository<RefreshToken> RefreshTokenRepository { get; }


        public UnitOfWork(NewsAggregatorContext context,
            IRepository<News> newsRepository,
            IRepository<Category> categoryRepository,
            IRepository<Subcategory> subcategoryRepository,
            IRepository<NewsStructure> newsStructureRepository,
            IRepository<NewsUrl> newsUrlRepository, 
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IRepository<RefreshToken> refreshTokenRepository,
            IRepository<UserRole> userRoleRepository)
        {
            _context = context;
            NewsRepository = newsRepository;
            CategoryRepository = categoryRepository;
            SubcategoryRepository = subcategoryRepository;
            NewsStructureRepository = newsStructureRepository;
            NewsUrlRepository = newsUrlRepository;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            RefreshTokenRepository = refreshTokenRepository;
            UserRoleRepository = userRoleRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
