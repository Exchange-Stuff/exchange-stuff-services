using ExchangeStuff.Core.Repositories.Base;
using System.Linq.Expressions;

namespace ExchangeStuff.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Task AddAsync(T item)
        {
            throw new NotImplementedException();
        }

        public List<T> GetManyAsync(Expression<Func<T, bool>> predicate = null, string? include = null, int? pageIndex = null, int? pageSize = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, string? include = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(T t)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<T> t)
        {
            throw new NotImplementedException();
        }
    }
}
