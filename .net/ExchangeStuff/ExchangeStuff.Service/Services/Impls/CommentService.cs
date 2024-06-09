using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Services.Interfaces;
using System.Net;

namespace ExchangeStuff.Service.Services.Impls;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _commentRepository = _unitOfWork.CommentRepository;
        _userRepository = _unitOfWork.UserRepository;
    }

    public async Task<ActionResult> CreateComment(CreateCommentModel request)
    {
        var actionResult = new ActionResult();
        //var product = await _productRepository.GetOneAsync(predicate: p => p.Id.Equals(request.ProductId));
        //if (product == null)
        //{
        //    throw new Exception("Not found!");
        //}
        var user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(request.AccountId));
        if (user == null) return actionResult.BuildError("Not found user", HttpStatusCode.NotFound);
        try
        {
            var comment = AutoMapperConfig.Mapper.Map<Comment>(request);
            await _commentRepository.AddAsync(comment);
            await _unitOfWork.SaveChangeAsync();
            return actionResult.BuildResult();
        } catch (Exception ex)
        {
            return actionResult.BuildError("Server error", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ActionResult> GetCommentByProductId(Guid id, int pageSize, int pageIndex)
    {
        var actionResult = new ActionResult();
        var comments = await _commentRepository.GetManyAsync(predicate: c => c.ProductId.Equals(id), pageIndex: pageIndex, pageSize: pageSize);
        var result = AutoMapperConfig.Mapper.Map<List<CommentViewModel>>(comments);
        return actionResult.BuildResult(result);
    }

    public async Task<ActionResult> UpdateComment(UpdateCommentModel request)
    {
        var actionResult = new ActionResult();
        var comment = await _commentRepository.GetOneAsync(predicate: c => c.Id.Equals(request.Id));
        if (comment == null) return actionResult.BuildError("Not found comment!", HttpStatusCode.NotFound);

        try
        {
            comment.Content = request.Content;
            _commentRepository.Update(comment);
            await _unitOfWork.SaveChangeAsync();
            return actionResult.BuildResult();
        } catch (Exception ex)
        {
            return actionResult.BuildError("Server error", HttpStatusCode.InternalServerError);
        }
    }
}
