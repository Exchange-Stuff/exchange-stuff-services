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
        private readonly IAuthService _authService;

        public ModeratorController(IAccountService accountService, IAuthService authService)
        {
            _accountService = accountService;
            _authService = authService;
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

        [HttpPost("moderator")]
        public async Task<IActionResult> CreateModerator(ModeratorCreateModel moderatorCreateModel)
        {
            var rs = await _authService.CreateModerator(moderatorCreateModel);
            return rs != null ? StatusCode(StatusCodes.Status201Created, new ResponseResult<ModeratorViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs
            }) : throw new Exception("Can't create moderator");
        }

        /// <summary>
        /// Delete user 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await _accountService.GetUser(id);
            if (user == null) throw new Exception("Not found user");
            var rs = await _authService.DeleteAccount(id);
            return rs ? StatusCode(StatusCodes.Status204NoContent) : throw new Exception("Can't delete account");
        }
    }
}
