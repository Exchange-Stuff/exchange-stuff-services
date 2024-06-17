using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Moderators;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeratorController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public ModeratorController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModerator([FromRoute] Guid id, bool? includeBan)
        => Ok(new ResponseResult<ModeratorViewModel>
        {
            Error = null!,
            IsSuccess = true,
            Value = await _accountService.GetModerator(id, includeBan)
        });

        [HttpGet]
        public async Task<IActionResult> GetModerators(string? name = null!, string? username = null!, string? email = null!, int? pageIndex = null!, int? pageSize = null!, bool? includeBan = null!)
           => Ok(new ResponseResult<List<ModeratorViewModel>>
           {
               Error = null!,
               IsSuccess = true,
               Value = await _accountService.GetModerators(name, email, username, pageIndex, pageSize, includeBan)
           });

    }
}
