using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KMS.Data.SeedWorks
{
    public interface IRepositoryBase<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        T Add(T entity);

        IEnumerable<T> AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        IQueryable<T> FindAll();

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);

        Task<T?> FindIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);

        Task<int> CountAsync(CancellationToken cancellationToken = default);

        Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }

    public class RepositoryBase<T> : IRepositoryBase<T>, IDisposable where T : class
    {
        private readonly DbSet<T> _dbSet;
        protected readonly KMSContext _context;

        public RepositoryBase(KMSContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public T Add(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            var addRange = entities as T[] ?? entities.ToArray();
            _dbSet.AddRange(addRange);
            return addRange;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IQueryable<T> FindAll()
        {
            IQueryable<T> items = _dbSet;
            return items;
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> items = _dbSet;
            return items.Where(predicate);
        }

        public virtual async Task<T?> FindIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty) return null;
            var data =  await _dbSet.FindAsync(new object[] { id }, cancellationToken);
            if (data is null) return null;
            return data;
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(where, cancellationToken);
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public virtual async Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }
    }
}