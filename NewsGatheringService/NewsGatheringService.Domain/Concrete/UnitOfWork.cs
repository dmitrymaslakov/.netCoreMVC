﻿using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Abstract;
using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsGatheringService.Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsAggregatorContext _context;

        public IRepository<Category> CategoryRepository { get; }
        public IRepository<Comment> CommentRepository { get; }
        public IRepository<News> NewsRepository { get; }
        public IRepository<NewsStructure> NewsStructureRepository { get; }
        public IRepository<Role> RoleRepository { get; }
        public IRepository<Subcategory> SubcategoryRepository { get; }
        public IRepository<User> UserRepository { get; }
        public IRepository<UserRole> UserRoleRepository { get; }

        public UnitOfWork(NewsAggregatorContext context,
            IRepository<News> newsRepository,
            IRepository<Category> categoryRepository,
            IRepository<Subcategory> subcategoryRepository,
            IRepository<NewsStructure> newsStructureRepository,
            IRepository<User> userRepository,
            IRepository<Role> roleRepository
            )
        {
            _context = context;
            NewsRepository = newsRepository;
            CategoryRepository = categoryRepository;
            SubcategoryRepository = subcategoryRepository;
            NewsStructureRepository = newsStructureRepository;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}