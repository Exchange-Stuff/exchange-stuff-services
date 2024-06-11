using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Rating;
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
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetRatingByProductId([FromRoute] Guid productId)
        {
            return Ok(new ResponseResult<List<RatingViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _ratingSerivce.GetRatingByProductId(productId)
            });
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRatingByUserId([FromRoute] Guid userId)
        {
            return Ok(new ResponseResult<List<RatingViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _ratingSerivce.GetRatingByUserId(userId)
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateRating(CreateRatingModel createModel)
        {
            var result = await _ratingSerivce.CreatRating(createModel);
            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = result.ToString()
            });
        }
        [HttpPut]
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
    }
}
