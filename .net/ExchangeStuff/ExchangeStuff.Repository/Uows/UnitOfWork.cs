using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories;

namespace ExchangeStuff.Repository.Uows
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExchangeStuffContext _context;

        public UnitOfWork(ExchangeStuffContext exchangeStuffContext)
        {
            _context = exchangeStuffContext;
            UserRepository = new UserRepository(_context);
            PurchaseTicketRepository = new PurchaseTicketRepository(_context);
        }

        public IUserRepository UserRepository { get; private set; }

        public IPurchaseTicketRepository PurchaseTicketRepository {  get; private set; }

        public async Task<int> SaveChangeAsync()
        => await _context.SaveChangesAsync();
    }
}
