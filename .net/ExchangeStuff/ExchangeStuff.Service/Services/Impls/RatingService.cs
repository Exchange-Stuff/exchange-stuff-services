using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Rating;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Paginations;
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

    public async Task<PaginationItem<RatingViewModel>> GetRatingByUserId(Guid userId, int pageSize, int pageIndex)
    {
        //var user = await _userRepo.GetOneAsync(u => u.Id.Equals(userId));
        //if (user == null) throw new Exception("Not found user!");
        var listPurchaseTicket = await _purchaseTicketRepo.GetManyAsync(
            include: "Rating",
            predicate: p => p.User.Equals(userId)
        );
        var result = AutoMapperConfig.Mapper.Map<List<RatingViewModel>>(listPurchaseTicket.Select(p => p.Rating));
        return PaginationItem<RatingViewModel>.ToPagedList(result, pageIndex, pageSize);
    }

    public async Task<bool> CreateRating(CreateRatingModel createRatingModel)
    {
        var createModel = AutoMapperConfig.Mapper.Map<Rating>(createRatingModel);
        await _ratingRepo.AddAsync(createModel);
        var result = await _unitOfWork.SaveChangeAsync();
        return result > 0;
    }

    public async Task<PaginationItem<RatingViewModel>> GetRatingByProductId(Guid productId, int pageSize, int pageIndex)
    {

        var product = await _productRepo.GetOneAsync(predicate: p => p.Id.Equals(productId));
        if (product == null) throw new Exception("Not found product!");

        var lisPurchase = await _purchaseTicketRepo.GetManyAsync(
            predicate: pu => pu.ProductId == product.Id,
            include: "Rating"
            );
        List<Rating> listRating = new List<Rating>();
        foreach (var purchase in lisPurchase)
        {
            if (purchase.Rating != null) 
            {
                listRating.Add(purchase.Rating);
            };
        }
        var result = AutoMapperConfig.Mapper.Map<List<RatingViewModel>>(listRating);
        return PaginationItem<RatingViewModel>.ToPagedList(result, pageIndex, pageSize);
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

        var avgRating = totalRating == 0 ? 0 : ((decimal)totalRating / ratingCount);
        return new RatingAvgViewModel { RatingAvg = Math.Round(avgRating, 2), RatingCount = ratingCount };
    }

    public async Task<RatingViewModel> GetRatingByPurchaseId(Guid id)
    {
        var purchase = await _purchaseTicketRepo.GetOneAsync(
            predicate: p => p.Id == id,
            include: "Rating");
        if (purchase == null) throw new Exception("Not found purchase");
        return AutoMapperConfig.Mapper.Map<RatingViewModel>(purchase.Rating);
    }
}