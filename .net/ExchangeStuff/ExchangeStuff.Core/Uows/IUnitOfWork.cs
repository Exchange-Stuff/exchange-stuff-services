using ExchangeStuff.Core.Repositories;

namespace ExchangeStuff.Core.Uows
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
