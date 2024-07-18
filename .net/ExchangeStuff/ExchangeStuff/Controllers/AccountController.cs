using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Paginations;
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


        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(string? name = null!, string? username = null!, string? email = null!, int? pageIndex = null!, int? pageSize = null!, bool? includeBan = null!)
            => Ok(new ResponseResult<PaginationItem<UserViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _accountService.GetUsers(name, email, username, pageIndex, pageSize, includeBan)
            });

        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id, bool? includeBan = null!)
            => Ok(new ResponseResult<UserViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _accountService.GetUser(id, includeBan)
            });

        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts(string? name = null!, string? username = null!, string? email = null!, int? pageIndex = null!, int? pageSize = null!, bool? includeBan = null!)
           => Ok(new ResponseResult<PaginationItem<AccountViewModel>>
           {
               Error = null!,
               IsSuccess = true,
               Value = await _accountService.GetAccounts(name, email, username, pageIndex, pageSize, includeBan)
           });

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] Guid id, bool? includeBan = null!)
            => Ok(new ResponseResult<AccountViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _accountService.GetAccount(id, includeBan)
            });

        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]
        [HttpPatch("update/user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateModel userUpdateModel)
        {
            return Ok(new ResponseResult<UserViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _accountService.UpdateUser(userUpdateModel)
            });
        }

    }
}
