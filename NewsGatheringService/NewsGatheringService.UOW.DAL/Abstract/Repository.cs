using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Interfaces;
using NewsGatheringService.UOW.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringService.UOW.DAL.Abstract
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
            try
            {
                return await DbSet.FirstOrDefaultAsync(news => news.Id.Equals(id), token);
            }
            catch
            {
                throw;
            }
        }

        public virtual IQueryable<T> GetAllAsQueryable()
        {
            try
            {
                return DbSet.AsQueryable();
            }
            catch
            {
                throw;
            }
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate = null, params Expression<Func<T, object>>[] includesPredicate)
        {
            try
            {
                var result = searchPredicate == null ? GetAllAsQueryable() : DbSet.Where(searchPredicate);

                if (includesPredicate.Any())
                {
                    result = includesPredicate
                        .Aggregate(result, (current, include) => current.Include(include));
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public virtual async Task AddAsync(T obj)
        {
            try
            {
                await DbSet.AddAsync(obj);
            }
            catch
            {
                throw;
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> objects)
        {
            try
            {
                await DbSet.AddRangeAsync(objects);
            }
            catch
            {
                throw;
            }
        }

        public void Update(T obj)
        {
            try
            {
                DbSet.Update(obj);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateRange(IEnumerable<T> obj)
        {
            try
            {
                DbSet.UpdateRange(obj);
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                DbSet.Remove(await DbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id)));
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteRange(IQueryable<Guid> ids)
        {
            try
            {
                var set = new List<T>();
                
                foreach (var id in ids)
                {
                    set.Add(await DbSet.FirstOrDefaultAsync(t => t.Id.Equals(id)));
                }

                DbSet.RemoveRange(set);
            }
            catch
            {
                throw;
            }
        }

    }
}
