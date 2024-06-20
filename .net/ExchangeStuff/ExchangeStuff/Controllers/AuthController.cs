using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Tokens;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPut("refresh")]
        public async Task<IActionResult> ReNewToken()
        {
            var rftk = (HttpContext.Request.Headers["RefreshToken"].First() + "").Split(" ").Last();
            if (string.IsNullOrEmpty(rftk)) throw new UnauthorizedAccessException("Refresh Token required");
            var rs = await _tokenService.RenewAccessToken(rftk);
            return rs != null ? Ok(new ResponseResult<TokenViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs
            }) : throw new Exception("Can't renew Token");
        }
    }
}
