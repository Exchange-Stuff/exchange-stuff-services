using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Accounts;

using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(string? name = null!, string? username = null!, string? email = null!, int? pageIndex = null!, int? pageSize = null!)
            => Ok(new ResponseResult<List<UserViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _accountService.GetUsers(name, email, username, pageIndex, pageSize)
            });

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)

            => Ok(new ResponseResult<UserViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _accountService.GetUser(id)
            });

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts(string? name = null!, string? username = null!, string? email = null!, int? pageIndex = null!, int? pageSize = null!)
           => Ok(new ResponseResult<List<AccountViewModel>>
           {
               Error = null!,
               IsSuccess = true,
               Value = await _accountService.GetAccounts(name, email, username, pageIndex, pageSize)
           });

        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] Guid id)
            => Ok(new ResponseResult<AccountViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _accountService.GetAccount(id)
            });

    }
}
