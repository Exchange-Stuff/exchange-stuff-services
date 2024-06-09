﻿using ExchangeStuff.Core.Repositories;

namespace ExchangeStuff.Core.Uows
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IUserRepository UserRepository { get; }
        IImageRepository ImageRepository { get; }
        IRatingRepository RatingRepository { get; }
        ICommentRepository CommentRepository { get; }
        ITokenRepository TokenRepository { get; }
        IAccountRepository AccountRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IActionRepository ActionRepository { get; }
        IPermissionGroupRepository PermissionGroupRepository { get; }
        IResourceRepository ResourceRepository { get; }
        IAdminRepository AdminRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
