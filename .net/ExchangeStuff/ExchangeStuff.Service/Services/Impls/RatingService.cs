using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Services.Interfaces;
using System.Net;

namespace ExchangeStuff.Service.Services.Impls;

public class RatingService : IRatingSerivce
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepo;
    private readonly IRatingRepository _ratingRepo;
    public RatingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepo = _unitOfWork.UserRepository;
        _ratingRepo = _unitOfWork.RatingRepository;
    }

    public async Task<ActionResult> GetRatingByUserId(Guid userId)
    {
        var actionResult = new ActionResult();

        var user = await _userRepo.GetOneAsync(u => u.Id.Equals(userId));
        if (user == null) return actionResult.BuildError("", HttpStatusCode.NotFound);

        //var ratings = await _ratingRepo.GetManyAsync(r => r.)
        return actionResult.BuildResult("123");
    }
}
