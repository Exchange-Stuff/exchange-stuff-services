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
            ProductRepository = new ProductRepository(_context);
            CategoriesRepository = new CategoriesRepository(_context);
        }

        public IUserRepository UserRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ICategoriesRepository CategoriesRepository { get; private set; }

        public async Task<int> SaveChangeAsync()
        => await _context.SaveChangesAsync();
    }
}
