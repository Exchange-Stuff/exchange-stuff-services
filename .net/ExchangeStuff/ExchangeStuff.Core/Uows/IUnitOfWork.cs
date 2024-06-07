using ExchangeStuff.Core.Repositories;

namespace ExchangeStuff.Core.Uows
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPurchaseTicketRepository PurchaseTicketRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
