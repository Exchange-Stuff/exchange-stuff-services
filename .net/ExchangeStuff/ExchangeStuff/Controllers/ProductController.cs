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
                return Ok();
            }

            return StatusCode(500, "A problem happened while handling your request.");
        }

    }
}
