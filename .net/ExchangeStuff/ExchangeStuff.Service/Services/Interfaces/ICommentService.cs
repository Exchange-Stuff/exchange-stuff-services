using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.Comments;
using ExchangeStuff.Service.Paginations;

namespace ExchangeStuff.Service.Services.Interfaces;

public interface ICommentService
{
    Task<PaginationItem<CommentViewModel>> GetCommentByProductId(Guid id, int pageSize, int pageIndex);
    Task<bool> CreateComment(CreateCommentModel request);
    Task<bool> UpdateComment(UpdateCommentModel request);
    Task<int> GetTotalCount(Guid productId);
}
