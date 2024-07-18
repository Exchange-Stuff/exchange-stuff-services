using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
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
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetCommentByProductId([FromRoute] Guid id, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            return Ok(new ResponseResult<List<CommentViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _commentService.GetCommentByProductId(id, pageSize, pageIndex)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.WRITE })]
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
        [ESAuthorize(new string[] { ActionConstant.READ })]
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
        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]
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
