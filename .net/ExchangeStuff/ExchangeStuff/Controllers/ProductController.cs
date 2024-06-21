using ExchangeStuff.Core.Enums;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Products;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _productService.GetAllProductsAsync();
            return Ok(product);
        }

        [HttpGet("getDetail/{id}")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            return Ok(new ResponseResult<ProductImageUserViewModel>
            {
                Error = null,
                IsSuccess = true,
                Value = await _productService.GetDetail(id)
            });
        }

        [HttpGet("/getProductByCategory/{categoryId}")]
        public async Task<IActionResult> GetProductByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryIdAsync(categoryId);
            return Ok(products);
        }

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
                return Ok(result);
            }

            return StatusCode(500, "A problem happened while handling your request.");
        }

        [HttpPut("updateStatusProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductViewModel updateProductViewModel)
        {
            var rs = await _productService.updateStatusProduct(updateProductViewModel);

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
