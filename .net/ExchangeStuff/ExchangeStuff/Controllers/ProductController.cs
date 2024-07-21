using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.Products;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly INotificationService _notiService;
        private readonly IProductService _productService;


        public ProductController(IProductService productService, INotificationService notificationService, IIdentityUser<Guid> identityUser)
        {
            _identityUser = identityUser;
            _notiService = notificationService;
            _productService = productService;
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _productService.GetAllProductsAsync();
            return Ok(product);
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getProductName/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            return Ok(new ResponseResult<List<ProductViewModel>>
            {
                Error = null,
                IsSuccess = true,
                Value = await _productService.GetProductByName(name)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getForModerator")]
        public async Task<IActionResult> GetForModerator()
        {
            var product = await _productService.GetListProductsForModerator();
            return Ok(product);
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getForAdmin")]
        public async Task<IActionResult> GetForAdmin()
        {
            var product = await _productService.GetListProductsForAdmin();
            return Ok(product);
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getDetail/{id}")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            return Ok(new ResponseResult<ProductViewModel>
            {
                Error = null,
                IsSuccess = true,
                Value = await _productService.GetDetail(id)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getProductByCategory/{categoryId}")]
        public async Task<IActionResult> GetProductByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryIdAsync(categoryId);
            return Ok(products);
        }
        [ESAuthorize(new string[] { ActionConstant.WRITE })]
        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await _productService.CreateProductAsync(model);
            if (result)
            {
                await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
                {
                    AccountId = _identityUser.AccountId,
                    Message = "Tạo sản phẩm thành công"
                });
                return Ok(result);
            }

            return StatusCode(500, "A problem happened while handling your request.");
        }
        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]
        [HttpPut("updateStatusProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductViewModel updateProductViewModel)
        {
            var rs = await _productService.updateStatusProduct(updateProductViewModel);
            if (!rs) throw new Exception("Can not update product");
            await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
            {
                AccountId = _identityUser.AccountId,
                Message = "Cập nhật sản phẩm thành công"
            });
            return StatusCode(StatusCodes.Status200OK, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getProductByUserId")]
        public async Task<IActionResult> GetProductUserId()
        {

            return Ok(new ResponseResult<List<ProductUserViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _productService.GetProductUser()
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getOtherUserProducts/{userId}")]
        public async Task<IActionResult> GetOtherUserProducts(Guid userId)
        {

            return Ok(new ResponseResult<List<ProductUserViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _productService.GetOtherUserProducts(userId)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]
        [HttpPut("cancelProduct/{productId}")]
        public async Task<IActionResult> CancelProduct(Guid productId)
        {
            var rs = await _productService.CancelProduct(productId);

            if (!rs) throw new Exception("Can not update product");

            return StatusCode(StatusCodes.Status200OK, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

    }
}
