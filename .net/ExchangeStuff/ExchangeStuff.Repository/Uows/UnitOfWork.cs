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
        }

        public IUserRepository UserRepository { get; private set; }

        public IImageRepository ImageRepository {  get; private set; }

        public ICommentRepository CommentRepository {  get; private set; }

        public IRatingRepository RatingRepository {  get; private set; }

        public async Task<int> SaveChangeAsync()
        => await _context.SaveChangesAsync();
    }
}
