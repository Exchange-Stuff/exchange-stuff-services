using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.Rating;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingSerivce _ratingSerivce;

        public RatingController(IRatingSerivce ratingSerivce)
        {
            _ratingSerivce = ratingSerivce;
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetRatingByProductId([FromRoute] Guid productId, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            return Ok(new ResponseResult<PaginationItem<RatingViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _ratingSerivce.GetRatingByProductId(productId, pageSize, pageIndex)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRatingByUserId([FromRoute] Guid userId,[FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            return Ok(new ResponseResult<PaginationItem<RatingViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _ratingSerivce.GetRatingByUserId(userId, pageSize, pageIndex)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("rating-avg-user/{id}")]
        public async Task<IActionResult> GetRatingAvg([FromRoute] Guid id)
        {
            return Ok(new ResponseResult<RatingAvgViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _ratingSerivce.GetRatingAvg(id)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.WRITE })]
        [HttpPost("create-rating")]
        public async Task<IActionResult> CreateRating(CreateRatingModel createModel)
        {
            var result = await _ratingSerivce.CreateRating(createModel);
            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = result.ToString()
            });
        }
        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]
        [HttpPut("update-rating")]
        public async Task<IActionResult> UpdateRating(UpdateRatingModel updateModel)
        {
            var result = await _ratingSerivce.UpdateRating(updateModel);
            return Ok(new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = result.ToString()
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("rating-by-purchase-id/{id}")]
        public async Task<IActionResult> GetRatingByPurchaseId([FromRoute] Guid id)
        {
            return Ok(new ResponseResult<RatingViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _ratingSerivce.GetRatingByPurchaseId(id)
            });
        }
    }
}
