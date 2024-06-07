using ExchangeStuff.Core.Repositories;

namespace ExchangeStuff.Core.Uows
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IImageRepository ImageRepository { get; }
        IRatingRepository RatingRepository { get; }
        ICommentRepository CommentRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
