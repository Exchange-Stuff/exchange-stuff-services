using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Tokens;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, IAuthService authService, ITokenService token)
        {
            _tokenService = token;
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        /// Signin Google
        /// </summary>
        /// <returns></returns>
        [HttpGet("signin")]
        public async Task<IActionResult> SigninGoogle()
        {
            var param = Request.QueryString + "";
            if (string.IsNullOrEmpty(param.Trim())) throw new Exception("Not found auth code");

            var tk = await _authService.GetToken(param);

            if (tk == null) throw new Exception("Can't get token");
            ResponseResult<TokenViewModel> responseResult = new ResponseResult<TokenViewModel>();
            responseResult.Error = null!;
            responseResult.IsSuccess = true;
            responseResult.Value = tk;
            return Ok(responseResult);
        }

        /// <summary>
        /// Provide access token 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var token = (HttpContext.Request.Headers.Authorization.FirstOrDefault() + "").Split(" ").Last();
            if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException("Not found token");
            ResponseResult<string> responseResult = new ResponseResult<string>();
            var rs = await _authService.Logout();
            if (rs)
            {
                return Ok(new ResponseResult<string>
                {
                    Error = null!,
                    IsSuccess = true,
                    Value = rs.ToString()
                });
            }
            throw new Exception("Server can't proccess logout");
        }

        /// <summary>
        /// Provide refreshtoken(Ex: Refresh) and accesstoken
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost("renew")]
        public async Task<IActionResult> ReNewToken([FromBody] RenewRd renewRd)
        {
            if (renewRd==null!|| string.IsNullOrEmpty(renewRd.RefreshToken)) throw new UnauthorizedAccessException("Refresh Token required");
            var rs = await _tokenService.RenewAccessToken(renewRd.RefreshToken);
            return rs != null ? Ok(new ResponseResult<TokenViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs
            }) : throw new Exception("Can't renew Token");
        }


        /// <summary>
        /// Login with username/email and password
        /// </summary>
        /// <param name="loginRd"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] LoginRd loginRd)
        {
            if (string.IsNullOrEmpty(loginRd.Username) || string.IsNullOrEmpty(loginRd.Password)) throw new Exception("Username and password required");
            return Ok(new ResponseResult<TokenViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _authService.LoginUsernameAndPwd(loginRd)
            });
        }

        /// <summary>
        /// Delete account (ignore admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            var rs = await _authService.DeleteAccount(id);
            return rs ? StatusCode(StatusCodes.Status204NoContent) : throw new Exception("Can't delete account");
        }

        /// <summary>
        /// Check screen (UserScreen, ModeratorScreen, AdminScreen) permisison, provide access token in header
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpPost("screen")]
        public async Task<IActionResult> ValidScreen([FromBody] ResourceRd resource)
        {
            if (resource == null! || string.IsNullOrEmpty(resource.Resource)) throw new Exception("Resource required");
            var rs = await _authService.ValidScreen(resource.Resource);
            return rs ? Ok(new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs + ""
            }) : throw new UnauthorizedAccessException("Access denial");
        }
    }

    public sealed record ResourceRd(string Resource);
    public sealed record RenewRd(string RefreshToken);
}
