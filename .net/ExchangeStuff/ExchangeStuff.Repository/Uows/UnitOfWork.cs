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
            ProductRepository = new ProductRepository(_context);
            CategoriesRepository = new CategoriesRepository(_context);
            PostTicketRepository = new PostTicketRepository(_context);
            PaymentRepository = new PaymentRepository(_context);
            PurchaseTicketRepository = new PurchaseTicketRepository(_context);
            TransactionHistoryRepository = new TransactionHistoryRepository(_context);
            ProductBanReportRepository = new ProductBanReportRepository(_context);
            UserBanReportRepository = new UserBanReportRepository(_context);
            BanReasonRepository = new BanReasonRepository(_context);
            ModeratorRepository = new ModeratorRepository(_context);    
            UserBalanceRepository = new UserBalanceRepository(_context);
        }

        public IUserRepository UserRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ICategoriesRepository CategoriesRepository { get; private set; }

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

        public IPurchaseTicketRepository PurchaseTicketRepository {  get; private set; }

        public ITransactionHistoryRepository TransactionHistoryRepository { get; private set; }
        public IPostTicketRepository PostTicketRepository { get; private set; }
        public IPaymentRepository PaymentRepository { get; private set; }

        public IProductBanReportRepository ProductBanReportRepository { get; private set; }

        public IUserBanReportRepository UserBanReportRepository { get; private set; }

        public IBanReasonRepository BanReasonRepository { get; private set; }

        public IModeratorRepository ModeratorRepository { get; private set; }
        public IUserBalanceRepository UserBalanceRepository { get; private set; }

        public async Task<int> SaveChangeAsync()
        => await _context.SaveChangesAsync();
    }
}
