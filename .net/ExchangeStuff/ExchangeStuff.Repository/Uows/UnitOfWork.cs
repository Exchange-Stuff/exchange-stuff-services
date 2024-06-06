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
            CategoryRepository = new CategoryRepository(_context);
            TokenRepository = new TokenRepository(_context);
            ActionRepository = new ActionRepository(_context);
        }

        public IUserRepository UserRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }

        public ITokenRepository TokenRepository { get; private set; }

        public IAccountRepository AccountRepository { get; private set; }

        public IPermissionRepository PermissionRepository { get; private set; }

        public IActionRepository ActionRepository { get; private set; }

        public async Task<int> SaveChangeAsync()
        => await _context.SaveChangesAsync();
    }
}
