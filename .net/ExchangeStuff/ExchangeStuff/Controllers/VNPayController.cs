using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VNPayController : ControllerBase
    {
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IVnPayService _vnPayService;

        public VNPayController(IVnPayService vnPayService,IIdentityUser<Guid> identityUser)
        {
            _identityUser=identityUser;
            _vnPayService = vnPayService;
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("createPaymentUrl")]
        public IActionResult CreatePayment([FromQuery] int amount)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(amount);
            return Ok(paymentUrl);
        }

    }
}
