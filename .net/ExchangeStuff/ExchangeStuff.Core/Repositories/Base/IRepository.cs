using System.Linq.Expressions;

namespace ExchangeStuff.Core.Repositories.Base
{
    public interface IRepository<T> where T :class
    {
        Task AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> entities);

        Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, string? include = null!);

        Task<List<T>> GetManyAsync(Expression<Func<T, bool>>? predicate = null!, string? include = null!, int? pageIndex = null, int? pageSize = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null!);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        void Update(T t);

        void UpdateRange(IEnumerable<T> t);
    }
}
