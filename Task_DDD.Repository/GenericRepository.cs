using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Task_DDD.Application.RepositoryContracts;

namespace Task_DDD.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;

        protected readonly DbSet<T> dbSet;

        public GenericRepository(DbContext context)
        {
            this._context = context ?? throw new ArgumentNullException("context");
            dbSet = context.Set<T>() ?? throw new ArgumentNullException("dbSet");
        }

        public async Task<T> Add(T entity)
        {
            await _context.AddAsync(entity);

            _context.SaveChanges();

            return entity;
        }
        public async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            return entity;
        }

        public async Task<bool> IsExist(Guid id)
        {
            var entity = await GetAsync(id);

            return entity != null;
        }

        public async Task<bool> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableGlobalFilter = false, bool disableTracking = true, bool disableMaxRowLimit = false)
        {
            IQueryable<T> queryable = dbSet.AsQueryable().TagWith("Repository GetQueryable");

            if (disableGlobalFilter)
            {
                queryable = queryable.IgnoreQueryFilters();
            }

            if (disableTracking)
            {
                queryable = queryable.AsNoTracking();
            }

            if (include != null)
            {
                queryable = include(queryable);
            }

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            if (!disableMaxRowLimit)
            {
                queryable = queryable.Take(1000);
            }

            return queryable;
        }

    
    }
}
