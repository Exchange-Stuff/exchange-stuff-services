using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.Comments;

namespace ExchangeStuff.Service.Services.Interfaces;

public interface ICommentService
{
    Task<List<CommentViewModel>> GetCommentByProductId(Guid id);
    Task<bool> CreateComment(CreateCommentModel request);
    Task<bool> UpdateComment(UpdateCommentModel request);
    Task<int> GetTotalCount(Guid productId);
}
