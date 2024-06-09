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
            CommentRepository = new CommentRepository(_context);
            ImageRepository = new ImageRepository(_context);
            RatingRepository = new RatingRepository(_context);
            FinancialTicketsRepository = new FinancialTicketsRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
            TokenRepository = new TokenRepository(_context);
            ActionRepository = new ActionRepository(_context);
            AccountRepository = new AccountRepository(_context);
            PermissionRepository = new PermissionRepository(_context);
            PermissionGroupRepository = new PermissionGroupRepository(_context);
            ResourceRepository = new ResourceRepository(_context);
            AdminRepository = new AdminRepository(_context);
        }

        public IUserRepository UserRepository { get; private set; }
        public IFinancialTicketsRepository FinancialTicketsRepository { get; private set; }

        public IImageRepository ImageRepository {  get; private set; }

        public ICommentRepository CommentRepository {  get; private set; }

        public IRatingRepository RatingRepository {  get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }

        public ITokenRepository TokenRepository { get; private set; }

        public IAccountRepository AccountRepository { get; private set; }

        public IPermissionRepository PermissionRepository { get; private set; }

        public IActionRepository ActionRepository { get; private set; }

        public IPermissionGroupRepository PermissionGroupRepository { get; private set; }

        public IResourceRepository ResourceRepository { get; private set; }

        public IAdminRepository AdminRepository { get; private set; }

        public async Task<int> SaveChangeAsync()
        => await _context.SaveChangesAsync();
    }
}
