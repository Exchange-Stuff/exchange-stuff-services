using ExchangeStuff.Core.Repositories;

namespace ExchangeStuff.Core.Uows
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IUserRepository UserRepository { get; }
        IImageRepository ImageRepository { get; }
        IRatingRepository RatingRepository { get; }
        ICommentRepository CommentRepository { get; }
        IFinancialTicketsRepository FinancialTicketsRepository { get; }
        ITokenRepository TokenRepository { get; }
        IAccountRepository AccountRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IActionRepository ActionRepository { get; }
        IPermissionGroupRepository PermissionGroupRepository { get; }
        IResourceRepository ResourceRepository { get; }
        IAdminRepository AdminRepository { get; }
        IProductRepository ProductRepository { get; }
        ICategoriesRepository CategoriesRepository { get; }
        IPurchaseTicketRepository PurchaseTicketRepository { get; }
        ITransactionHistoryRepository TransactionHistoryRepository { get; }
        IPostTicketRepository PostTicketRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IProductBanReportRepository ProductBanReportRepository { get; }
        IUserBanReportRepository UserBanReportRepository { get; }
        IBanReasonRepository BanReasonRepository { get; }
        IModeratorRepository ModeratorRepository { get; }
        IUserBalanceRepository UserBalanceRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IMessageChatRepository MessageChatRepository { get; }
        IGroupChatRepository GroupChatRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
