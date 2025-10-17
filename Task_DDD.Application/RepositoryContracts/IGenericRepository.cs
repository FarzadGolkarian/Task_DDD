using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Task_DDD.Application.RepositoryContracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task<T> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> IsExist(Guid id);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>>
            predicate = null, Func<IQueryable<T>, IQueryable<T>>
            orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>
            include = null, bool disableGlobalFilter = false, bool disableTracking = true, bool disableMaxRowLimit = false);

    }
}
