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
        //var user = await _userRepo.GetOneAsync(u => u.Id.Equals(userId));
        //if (user == null) throw new Exception("Not found user!");
        var listPurchaseTicket = await _purchaseTicketRepo.GetManyAsync(include: "Rating", predicate: p => p.User.Equals(userId));
        var result = AutoMapperConfig.Mapper.Map<List<RatingViewModel>>(listPurchaseTicket.Select(p => p.Rating));
        return result;
    }

    public async Task<bool> CreateRating(CreateRatingModel createRatingModel)
    {
        var createModel = AutoMapperConfig.Mapper.Map<Rating>(createRatingModel);
        await _ratingRepo.AddAsync(createModel);
        var result = await _unitOfWork.SaveChangeAsync();
        return result > 0;
    }

    public async Task<List<RatingViewModel>> GetRatingByProductId(Guid productId)
    {
        //var product = await _productRepo.GetOneAsync(predicate: p => p.Id.Equals(productId));
        //if (product == null) throw new Exception("Not found product!");

        //var listPurchaseTicket = await _purchaseTicketRepo.GetManyAsync(predicate: p => p.ProductId.Equals(productId));
        //var result = AutoMapperConfig.Mapper.Map<List<RatingViewModel>>(listPurchaseTicket.SelectMany(p => p.Ratings));
        //return result;
        throw new NotImplementedException();

        var listPurchaseTicket = await _purchaseTicketRepo.GetManyAsync(predicate: p => p.ProductId.Equals(productId));
        var result = AutoMapperConfig.Mapper.Map<List<RatingViewModel>>(listPurchaseTicket.Select(p => p.Rating));
        return result;
    }

    public async Task<bool> UpdateRating(UpdateRatingModel updateRatingModel)
    {
        var updateModel = AutoMapperConfig.Mapper.Map<Rating>(updateRatingModel);
        _ratingRepo.Update(updateModel);
        var result = await _unitOfWork.SaveChangeAsync();
        return result > 0;
    }

    public async Task<RatingAvgViewModel> GetRatingAvg(Guid userId)
    {
        // check user existing
        var user = await _userRepo.GetOneAsync(predicate: u => u.Id.Equals(userId));
        if (user == null) throw new Exception("Not found user!");

        // get list product create by user
        var listProduct = await _productRepo.GetManyAsync(predicate: p => p.CreatedBy == user.Id);

        // get list rating
        List<Rating> listRating = new List<Rating>();
        foreach (var product in listProduct)
        {
            var purchaseTicket = await _purchaseTicketRepo.GetOneAsync(predicate: pt => pt.ProductId == product.Id, include: "Rating");
            if (purchaseTicket != null && purchaseTicket.Rating != null)
                listRating.Add(purchaseTicket.Rating);
        }
        var ratingCount = listRating.Count;
        var totalRating = listRating.Sum(p => (int)p.EvaluateType);

        var avgRating = (decimal)totalRating / ratingCount;
        return new RatingAvgViewModel { RatingAvg = Math.Round(avgRating, 2), RatingCount = ratingCount };
    }
}