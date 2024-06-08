using ExchangeStuff.Core.Repositories;

namespace ExchangeStuff.Core.Uows
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IAccountRepository AccountRepository { get; }
        IPurchaseTicketRepository PurchaseTicketRepository { get; }
        ITransactionHistoryRepository TransactionHistoryRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
