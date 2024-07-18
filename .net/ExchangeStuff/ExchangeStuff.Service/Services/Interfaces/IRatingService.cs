﻿using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Interfaces;

public interface IRatingSerivce
{
    Task<List<RatingViewModel>> GetRatingByUserId(Guid userId, int pageSize, int pageIndex);
    Task<List<RatingViewModel>> GetRatingByProductId(Guid productId, int pageSize, int pageIndex);
    Task<bool> CreateRating(CreateRatingModel createRatingModel);
    Task<bool> UpdateRating(UpdateRatingModel updateRatingModel);
    Task<RatingAvgViewModel> GetRatingAvg(Guid userId);
}
