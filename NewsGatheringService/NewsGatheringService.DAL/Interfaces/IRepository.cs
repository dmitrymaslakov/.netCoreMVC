using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringService.DAL.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> GetByIdAsync(Guid id, CancellationToken token);

        IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate,
            params Expression<Func<T, object>>[] includesPredicate);

        Task AddAsync(T obj);

        Task AddRangeAsync(IEnumerable<T> objects);

        void Update(T obj);

        Task DeleteAsync(Guid id);

        Task DeleteRange(IQueryable<Guid> ids);

        IQueryable<T> GetAllAsync(params Expression<Func<T, object>>[] includesPredicate);
    }
}
