using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public ManagementController(IAccountService accountService)
        {
            _accountService = accountService;
        }
    }
}
