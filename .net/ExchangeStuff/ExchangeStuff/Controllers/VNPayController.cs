using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VNPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VNPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public IActionResult CreatePayment([FromQuery] int amount)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(amount);
            return Redirect(paymentUrl);
        }
    }
}
