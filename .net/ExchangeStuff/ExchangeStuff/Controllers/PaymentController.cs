using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Products;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpGet("createPayment")]
        public async Task<IActionResult> createPayment([FromQuery] Guid userId, [FromQuery] int amount)
        {
            var rs = await _paymentService.createPaymentAsync(userId, amount);

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
