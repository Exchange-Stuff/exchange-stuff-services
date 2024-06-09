using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Services.Interfaces;

public interface ICommentService
{
    Task<ActionResult> GetCommentByProductId(Guid id, int pageSize, int pageIndex);
    Task<ActionResult> CreateComment(CreateCommentModel request);
    Task<ActionResult> UpdateComment(UpdateCommentModel request);
}
