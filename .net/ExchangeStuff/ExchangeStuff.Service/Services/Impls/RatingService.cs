using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Rating;
using ExchangeStuff.Service.Services.Interfaces;
using System.Net;

namespace ExchangeStuff.Service.Services.Impls;

public class RatingService : IRatingSerivce
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepo;
    private readonly IRatingRepository _ratingRepo;
    private readonly IProductRepository _productRepo;
    private readonly IPurchaseTicketRepository _purchaseTicketRepo;
    public RatingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepo = _unitOfWork.UserRepository;
        _ratingRepo = _unitOfWork.RatingRepository;
        _purchaseTicketRepo = _unitOfWork.PurchaseTicketRepository;
        _productRepo = _unitOfWork.ProductRepository;
    }

    public async Task<List<RatingViewModel>> GetRatingByUserId(Guid userId)
    {
        var user = await _userRepo.GetOneAsync(u => u.Id.Equals(userId));
        if (user == null) throw new Exception("Not found user!");

        var listPurchaseTicket = await _purchaseTicketRepo.GetManyAsync(include: "Ratings", predicate: p => p.User.Equals(userId));
        var result = AutoMapperConfig.Mapper.Map<List<RatingViewModel>>(listPurchaseTicket.SelectMany(p => p.Ratings));
        return result;
    }

    public async Task<bool> CreatRating(CreateRatingModel createRatingModel)
    {
        var createModel = AutoMapperConfig.Mapper.Map<Rating>(createRatingModel);
        await _ratingRepo.AddAsync(createModel);
        var result = await _unitOfWork.SaveChangeAsync();
        return result > 0;
    }

    public async Task<List<RatingViewModel>> GetRatingByProductId(Guid productId)
    {
        var product = await _productRepo.GetOneAsync(predicate: p => p.Id.Equals(productId));
        if (product == null) throw new Exception("Not found product!");

        var listPurchaseTicket = await _purchaseTicketRepo.GetManyAsync(predicate: p => p.ProductId.Equals(productId));
        var result = AutoMapperConfig.Mapper.Map<List<RatingViewModel>>(listPurchaseTicket.SelectMany(p => p.Ratings));
        return result;
    }

    public async Task<bool> UpdateRating(UpdateRatingModel updateRatingModel)
    {
        var updateModel = AutoMapperConfig.Mapper.Map<Rating>(updateRatingModel);
        _ratingRepo.Update(updateModel);
        var result = await _unitOfWork.SaveChangeAsync();
        return result > 0;
    }
}
