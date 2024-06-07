using ExchangeStuff.Core.Repositories;

namespace ExchangeStuff.Core.Uows
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        ICategoriesRepository CategoriesRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
