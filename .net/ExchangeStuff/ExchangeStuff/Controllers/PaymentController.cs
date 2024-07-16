using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.Products;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

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

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("createPayment")]
        public async Task<IActionResult> createPayment([FromQuery] Guid userId, [FromQuery] int amount)
        {
            var rs = await _paymentService.createPaymentAsync(userId, amount);

            if (!rs) throw new Exception("Can not create payment");

            var redirectUrl = $"http://localhost:3000/payment?status=success";
            return Redirect(redirectUrl);
        }
    }
}
