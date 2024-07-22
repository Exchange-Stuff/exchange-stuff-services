using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Comments;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _commentRepository = _unitOfWork.CommentRepository;
        _userRepository = _unitOfWork.UserRepository;
        _productRepository = _unitOfWork.ProductRepository;
        _accountRepository = _unitOfWork.AccountRepository;
    }

    public async Task<bool> CreateComment(CreateCommentModel request)
    {
        var product = await _productRepository.GetOneAsync(predicate: p => p.Id.Equals(request.ProductId));
        if (product == null) throw new Exception("Not found product!");

        var user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(request.AccountId));
        if (user == null) throw new Exception("Not found user!");

        var comment = AutoMapperConfig.Mapper.Map<Comment>(request);
        await _commentRepository.AddAsync(comment);
        var result = await _unitOfWork.SaveChangeAsync();
        return result > 0;
    }

    public async Task<PaginationItem<CommentViewModel>> GetCommentByProductId(Guid id, int pageSize, int pageIndex)
    {
        var comments = await _commentRepository.GetManyAsync(
            predicate: c => c.ProductId.Equals(id),
            orderBy: c => c.OrderByDescending(c => c.CreatedOn),
            include: "User"
        );
        var result = AutoMapperConfig.Mapper.Map<List<CommentViewModel>>(comments);
        return PaginationItem<CommentViewModel>.ToPagedList(result, pageIndex, pageSize);
    }

    public async Task<int> GetTotalCount(Guid productId)
    {
        var product = await _productRepository.GetOneAsync(predicate: p => p.Id == productId);
        if (product == null) throw new Exception("Not found product!");
        var comments = await _commentRepository.GetManyAsync(predicate: c => c.ProductId == product.Id);
        return comments.Count();
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
