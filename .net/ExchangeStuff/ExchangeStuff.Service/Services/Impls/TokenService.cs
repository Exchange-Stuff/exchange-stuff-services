using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExchangeStuff.Service.Services.Impls
{
    public class TokenService : CacheService, ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IUnitOfWork _uow;
        private readonly ITokenRepository _tokenRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private RefreshTokenDTO _refreshTokenDTO = new();
        private JwtDTO _jwtDTO = new();
        private GoogleAuthDTO _googleAuthDTO = new();

        public TokenService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IDistributedCache distributed, IConnectionMultiplexer connectionMultiplexer) : base(unitOfWork, distributed, connectionMultiplexer, configuration)
        {
            _connectionMutiple = connectionMultiplexer;
            _distributedCache = distributed;
            _configuration = configuration;
            _httpAccessor = httpContextAccessor;
            _uow = unitOfWork;
            _tokenRepository = _uow.TokenRepository;
            _accountRepository = _uow.AccountRepository;
            _userRepository = _uow.UserRepository;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
            _configuration.GetSection(nameof(RefreshTokenDTO)).Bind(_refreshTokenDTO);
            _configuration.GetSection(nameof(GoogleAuthDTO)).Bind(_googleAuthDTO);
        }

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpAccessor = httpContextAccessor;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
            _configuration.GetSection(nameof(RefreshTokenDTO)).Bind(_refreshTokenDTO);
            _configuration.GetSection(nameof(GoogleAuthDTO)).Bind(_googleAuthDTO);
        }

        public async Task<bool> CheckRefreshTokenExpire(string token)
        {
            var rt = (await _tokenRepository.GetManyAsync(x => x.RefreshToken == token)).FirstOrDefault();
            if (rt == null) throw new UnauthorizedAccessException("Refresh token invalid");
            var time = rt.CreatedOn.AddMinutes(_refreshTokenDTO.ExpireMinute);
            if (time < DateTime.Now)
            {
                return false;
            }
            return true;
        }

        public async Task<string> GenerateToken(Account account)
        {
            var key = Encoding.UTF8.GetBytes(_jwtDTO.JwtSecret);
            var tokenDescribe = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, account.Id + ""),
                    new Claim(ClaimTypes.Email, account.Email??"")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Audience = _jwtDTO.Audience,
                Issuer = _jwtDTO.Issuer,
                Expires = DateTime.Now.AddMinutes(_jwtDTO.ExpireMinute),
            };
            var tokenHandle = new JwtSecurityTokenHandler();
            var token = tokenHandle.CreateToken(tokenDescribe);
            await Task.CompletedTask;
            return tokenHandle.WriteToken(token);
        }

        public async Task<string> GenerateToken(Admin admin)
        {
            var key = Encoding.UTF8.GetBytes(_jwtDTO.JwtSecret);
            var tokenDescribe = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id + ""),
                   new Claim(ClaimTypes.Email, admin.Email??"")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Audience = _jwtDTO.Audience,
                Issuer = _jwtDTO.Issuer,
                Expires = DateTime.Now.AddMinutes(_jwtDTO.ExpireMinute),
            };

            var tokenHandle = new JwtSecurityTokenHandler();
            var token = tokenHandle.CreateToken(tokenDescribe);
            await Task.CompletedTask;
            return tokenHandle.WriteToken(token);
        }

        public async Task<ClaimDTO> GetClaimDTOByAccessToken(string? token = null)
        {
            try
            {
                if (token == null)
                {
                    token = (_httpAccessor.HttpContext.Request.Headers["Authorization"].First() + "").Split(" ").Last();
                }
            }
            catch
            {
                throw new UnauthorizedAccessException("Login session has expired");
            }
            if (token == null!) throw new UnauthorizedAccessException("Not found token, login please");
            try
            {
                var keyExample = _jwtDTO.JwtSecret;
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyExample)),
                    ValidateIssuer = false, //**
                    ValidateAudience = false, //** 
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _jwtDTO.Issuer,
                    ValidAudience = _jwtDTO.Audience,
                }, out SecurityToken securityToken);
                var jwtToken = (JwtSecurityToken)securityToken;
                var id = jwtToken.Claims.First(x => x.Type == "nameid")!.Value;
                var email = jwtToken.Claims.First(x => x.Type == "email")!.Value;

                if (Guid.TryParse(id, out Guid newId) is false
                    )
                {
                    return null!;
                }
                await Task.CompletedTask;
                return new ClaimDTO
                {
                    Id = newId,
                    Email = email
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public ClaimDTO GetClaimDTOByAccessTokenSynchronous(string? token = null)
        {
            try
            {
                if (token == null)
                {
                    token = (_httpAccessor.HttpContext.Request.Headers["Authorization"].First() + "").Split(" ").Last();
                }
            }
            catch
            {
                return null!;
            }
            if (token == null!) return null!;
            try
            {
                var keyExample = _jwtDTO.JwtSecret;
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyExample)),
                    ValidateIssuer = false, //**
                    ValidateAudience = false, //** 
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _jwtDTO.Issuer,
                    ValidAudience = _jwtDTO.Audience,

                }, out SecurityToken securityToken);
                var jwtToken = (JwtSecurityToken)securityToken;
                var id = jwtToken.Claims.First(x => x.Type == "nameid")!.Value;
                var email = jwtToken.Claims.First(x => x.Type == "email")!.Value;

                if (Guid.TryParse(id, out Guid newId) is false ||
                    string.IsNullOrEmpty(email)
                    )
                {
                    return null!;
                }

                return new ClaimDTO
                {
                    Id = newId,
                    Email = email
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> RenewAccessToken(string refreshToken)
        {
            var token = (await _tokenRepository.GetManyAsync(x => x.RefreshToken == refreshToken)).FirstOrDefault();
            if (token == null!)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            if (token.ModifiedOn.AddMinutes(_jwtDTO.ExpireMinute) > DateTime.Now)
            {
                throw new UnauthorizedAccessException("Token still valid");
            }
            if (await CheckRefreshTokenExpire(token.RefreshToken) is false)
            {
                await DeleteAccessToken(token.RefreshToken);
                throw new SecurityTokenExpiredException("Refresh token expired");
            }
            var account = await _accountRepository.GetOneAsync(x => x.Id == token.ModifiedBy);
            if (account == null)
            {
                throw new ArgumentNullException("Not found user");
            }
            string oldToken = token.AccessToken;
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!)
            {
                throw new UnauthorizedAccessException();
            }
            var tokenrf = (await _tokenRepository.GetManyAsync(x => x.RefreshToken == refreshToken)).FirstOrDefault();
            if (tokenrf == null) throw new UnauthorizedAccessException("Not found token");
            tokenrf.RefreshToken = GenerateRefreshToken();
            tokenrf.AccessToken = await GenerateToken(account);
            _tokenRepository.Update(tokenrf);
            var rs = await _uow.SaveChangeAsync();
            if (rs > 0)
            {
                await _distributedCache.RemoveAsync(oldToken);
                await _distributedCache.SetStringAsync(tokenrf.AccessToken, tokenrf.ModifiedBy + "");
                return tokenrf.AccessToken;
            }
            return null!;
        }

        public string HashPassword(string pwd)
        {
            using (SHA256 hasher = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(pwd);
                byte[] hashedBytes = hasher.ComputeHash(inputBytes);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
    }
}
