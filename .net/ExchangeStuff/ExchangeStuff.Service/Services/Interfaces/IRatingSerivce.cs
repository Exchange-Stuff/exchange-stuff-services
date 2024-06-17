using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Interfaces;

public interface IRatingSerivce
{
    Task<List<RatingViewModel>> GetRatingByUserId(Guid userId);
    Task<List<RatingViewModel>> GetRatingByProductId(Guid productId);
    Task<bool> CreatRating(CreateRatingModel createRatingModel);
    Task<bool> UpdateRating(UpdateRatingModel updateRatingModel);
}
