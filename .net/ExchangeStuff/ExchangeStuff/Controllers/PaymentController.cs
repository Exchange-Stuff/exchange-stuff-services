using ExchangeStuff.Responses;
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


        [HttpGet("createPayment")]
        public async Task<IActionResult> CreatePayment([FromQuery] Guid userId, [FromQuery] int amount, [FromQuery] int vnp_ResponseCode)
        {
            if (vnp_ResponseCode == 24)
            {
                var redirectUrl = $"http://localhost:3000/payment?status=fail";
                return Redirect(redirectUrl);
            }
            else if (vnp_ResponseCode == 00)
            {
                var rs = await _paymentService.createPaymentAsync(userId, amount);

                if (!rs) throw new Exception("Can not create payment");

                var redirectUrl = $"http://localhost:3000/payment?status=success";
                return Redirect(redirectUrl);
            }
            else
            {
                var redirectUrl = $"http://localhost:3000/payment?status=unknown";
                return Redirect(redirectUrl);
            }
        }

    }
}
