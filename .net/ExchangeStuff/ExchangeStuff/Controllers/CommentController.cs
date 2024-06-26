using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Comments;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetCommentByProductId([FromRoute] Guid id, int? pageSize, int? pageIndex)
        {
            return Ok(new ResponseResult<List<CommentViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _commentService.GetCommentByProductId(id, pageSize, pageIndex)
            });
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateComment(CreateCommentModel createModel)
        {
            bool result = await _commentService.CreateComment(createModel);
            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = result.ToString()
            });
        }
        [HttpGet("get-total-count/{id}")]
        public async Task<IActionResult> GetTotalCount(Guid id)
        {
            var result = await _commentService.GetTotalCount(id);
            return Ok(new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = result.ToString(),
            });
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment(UpdateCommentModel updateModel)
        {
            bool result = await _commentService.UpdateComment(updateModel);
            return Ok(new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = result.ToString()
            });
        }
    }
}
