using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.CurrentUser.Users;
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
        private readonly INotificationService _notiService;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService, IProductService productService, IIdentityUser<Guid> identityUser, INotificationService notificationService)
        {
            _notiService = notificationService;
            _identityUser = identityUser;
            _productService = productService;
            _commentService = commentService;
        }

        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetCommentByProductId([FromRoute] Guid id)
        {
            return Ok(new ResponseResult<List<CommentViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _commentService.GetCommentByProductId(id)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.WRITE })]

        [HttpPost("create")]
        public async Task<IActionResult> CreateComment(CreateCommentModel createModel)
        {
            bool result = await _commentService.CreateComment(createModel);
            var prod = await _productService.GetDetail(createModel.ProductId);
            if (prod != null)
            {
                await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
                {
                    AccountId = prod.CreatedBy,
                    Message = $"Có một bình luận mới từ bài viết {prod.Name.Substring(0, 15)}...."
                });
            }
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
