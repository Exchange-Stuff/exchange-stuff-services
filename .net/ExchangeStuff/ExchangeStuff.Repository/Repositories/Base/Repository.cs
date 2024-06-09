using ExchangeStuff.Core.Repositories.Base;
using ExchangeStuff.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExchangeStuff.Repository.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _entities;

        public Repository(ExchangeStuffContext context)
        {
            _entities = context.Set<T>();
        }

        public async Task AddAsync(T item)
        {
            await _entities.AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public async Task<List<T>> GetManyAsync(Expression<Func<T, bool>>? predicate = null, string? include = null, int? pageIndex = null, int? pageSize = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _entities;
            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item.Trim()).AsNoTracking();
                }
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                query = query.Skip(index * size).Take(size);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, string? include = null)
        {
            IQueryable<T> query = _entities;
            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item).AsNoTracking();
                }
            }
            return (await query.FirstOrDefaultAsync(predicate))!;
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }

        public void Update(T t)
        {
            var entry = _entities.Entry(t);
            if (entry.State==EntityState.Detached)
            {
                _entities.Attach(t);
            }
            entry.State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<T> t)
        {
            foreach (var entity in t)
            {
                var entry = _entities.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    _entities.Attach(entity);
                }
                entry.State = EntityState.Modified;
            }
        }
    }
}
