using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringService.DAL.Abstract
{
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbSet<T> DbSet;

        protected Repository(NewsAggregatorContext context)
        {
            DbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(Guid id, CancellationToken token)
        {

            return await DbSet.FirstOrDefaultAsync(news => news.Id.Equals(id), token);
        }
        public virtual IQueryable<T> GetAllAsync(params Expression<Func<T, object>>[] includesPredicate)
        {
            var result = DbSet.Select(e => e);
            if (includesPredicate.Any())
            {
                result = includesPredicate
                    .Aggregate(result, (current, include) => current.Include(include));
            }

            return result;
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate, params Expression<Func<T, object>>[] includesPredicate)
        {
            var result = DbSet.Where(searchPredicate);

            if (includesPredicate.Any())
            {
                result = includesPredicate
                    .Aggregate(result, (current, include) => current.Include(include));
            }

            return result;
        }

        public virtual async Task AddAsync(T obj)
        {
            await DbSet.AddAsync(obj);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> objects)
        {
            await DbSet.AddRangeAsync(objects);
        }

        public void Update(T obj)
        {
            DbSet.Update(obj);
        }

        public async Task DeleteAsync(Guid id)
        {
            DbSet.Remove(await DbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id)));
        }

        public async Task DeleteRange(IQueryable<Guid> ids)
        {
            var set = new List<T>();
            foreach (var id in ids)
            {
                set.Add(await DbSet.FirstOrDefaultAsync(t => t.Id.Equals(id)));
            }

            DbSet.RemoveRange(set);
        }

    }
}
