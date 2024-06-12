using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Comments;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _commentRepository = _unitOfWork.CommentRepository;
        _userRepository = _unitOfWork.UserRepository;
        _productRepository = _unitOfWork.ProductRepository;
    }

    public async Task<bool> CreateComment(CreateCommentModel request)
    {
        var product = await _productRepository.GetOneAsync(predicate: p => p.Id.Equals(request.ProductId));
        if (product == null) throw new Exception("Not found!");

        var user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(request.AccountId));
        if (user == null) throw new Exception("Not found user");

        var comment = AutoMapperConfig.Mapper.Map<Comment>(request);
        await _commentRepository.AddAsync(comment);
        var result = await _unitOfWork.SaveChangeAsync();
        return result > 0;
    }

    public async Task<List<CommentViewModel>> GetCommentByProductId(Guid id, int? pageSize, int? pageIndex)
    {
        var comments = await _commentRepository.GetManyAsync(predicate: c => c.ProductId.Equals(id), pageIndex: pageIndex, pageSize: pageSize);
        var result = AutoMapperConfig.Mapper.Map<List<CommentViewModel>>(comments);
        return result;
    }

    public async Task<bool> UpdateComment(UpdateCommentModel request)
    {
        var comment = await _commentRepository.GetOneAsync(predicate: c => c.Id.Equals(request.Id));
        if (comment == null) throw new Exception("Not found comment!");

        comment.Content = request.Content;
        _commentRepository.Update(comment);
        var result = await _unitOfWork.SaveChangeAsync();
        return result > 0;
    }
}
