using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Tokens;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;

namespace ExchangeStuff.Service.Services.Impls
{
    public class AuthService : TokenService, IAuthService
    {
        private readonly IConfiguration _configuration;
        private GoogleAuthDTO _googleAuthDTO = new();
        private readonly IHttpContextAccessor _httpConextAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPermissionGroupRepository _permissionGroupRepository;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, configuration, httpContextAccessor, distributedCache, connectionMultiplexer)
        {
            _uow = unitOfWork;
            _httpConextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
            _configuration = configuration;
            _configuration.GetSection(nameof(GoogleAuthDTO)).Bind(_googleAuthDTO);
            _userRepository = _uow.UserRepository;
            _accountRepository = _uow.AccountRepository;
            _permissionGroupRepository = _uow.PermissionGroupRepository;
        }

        public async Task<TokenViewModel> GetToken(string param)
        {
            if (param != "")
            {
                param = param.Substring(1);
                string[] splitParam = param.Split('&', StringSplitOptions.RemoveEmptyEntries);
                string code = "";
                bool finded = false;
                bool isFpt = false;
                foreach (var item in splitParam)
                {
                    if (!finded)
                    {
                        code = item.Split('=')[0] + "";
                    }
                    if (code == "code")
                    {
                        code = item.Split('=')[1] + "";
                        finded = true;
                    }
                    if (item.Split('=')[0] + "" == "hd")
                    {
                        isFpt = item.Split('=')[1] + "" == "fpt.edu.vn";
                    }
                }
                if (!isFpt)
                {
                    throw new UnauthorizedAccessException("Allow only email fpt");
                }
                if (!string.IsNullOrEmpty(code.Trim()))
                {
                    var token = await GetAccessTokenAsync(code);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var userindor = GetAuth(token);
                        if (userindor == null!) throw new UnauthorizedAccessException("UserInfor invalid");


                        var acc = await UserSigninCreate(userindor);
                        if (acc == null) throw new UnauthorizedAccessException("Not found user");
                        var tokenSystem = await GenerateToken(acc);

                        await SavePermissionGroup(acc.Id);
                        var ntoken = await SaveAccessToken(tokenSystem, acc.Id);
                        return AutoMapperConfig.Mapper.Map<TokenViewModel>(ntoken);
                    }
                }
                throw new UnauthorizedAccessException("Not found auth code");
            }
            return null!;
        }

        public async Task<Account> UserSigninCreate(UserGGInfo userinfo)
        {
            if (userinfo != null!)
            {
                Account account = await _accountRepository.GetAccountByEmail(userinfo.Email);
                if (account == null!)
                {
                    User user = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = userinfo.Email,
                        Name = userinfo.Name,
                        Thumbnail = userinfo.Thumbnail,
                        IsActived = true
                    };
                    var permissionGroups = await _permissionGroupRepository.GetManyAsync(x => x.Name == GroupPermission.DEFAULT, forUpdate: true);
                    if (permissionGroups.Any() is false) throw new Exception("No permission in system");

                    user.PermissionGroups = permissionGroups;
                    await _userRepository.AddAsync(user);

                    var rs = await _uow.SaveChangeAsync();
                    if (rs > 0)
                    {
                        account = await _accountRepository.GetAccountByEmail(userinfo.Email);
                    }
                    else
                    {
                        throw new Exception("Can't create new user");
                    }
                }
                else
                {
                    account.Thumbnail = userinfo.Thumbnail != account.Thumbnail ? userinfo.Thumbnail : account.Thumbnail;
                    account.Name = userinfo.Name != account.Name ? userinfo.Name : account.Name;
                    await _uow.SaveChangeAsync();
                }
                return account;
            }
            return null!;
        }
        public async Task<bool> Logout()
        {
            var tokenCt = _httpConextAccessor.HttpContext.Request.Headers["Authorization"].First()!.Split(" ").Last();
            return await DeleteAccessToken(tokenCt);
        }
        private UserGGInfo GetAuth(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jsonToken != null)
            {
                var email = jsonToken.Claims.First(claim => claim.Type == "email").Value;
                var name = jsonToken.Claims.First(claim => claim.Type == "name").Value;
                var thumbnail = jsonToken.Claims.First(claim => claim.Type == "picture").Value ?? "";
                return new UserGGInfo(email, name, thumbnail);
            }
            return null!;
        }

        private async Task<string> GetAccessTokenAsync(string code)
        {
            code = code.Replace("%2F", "/");
            string url = "https://oauth2.googleapis.com/token";
            var dicData = new Dictionary<string, string>();
            dicData["client_id"] = _googleAuthDTO.ClientId;
            dicData["client_secret"] = _googleAuthDTO.ClientSecret;
            dicData["code"] = code;
            dicData["grant_type"] = "authorization_code";
            dicData["redirect_uri"] = _googleAuthDTO.ReturnUrl;
            dicData["access_type"] = "online";
            try
            {
                using (var client = new HttpClient())
                using (var content = new FormUrlEncodedContent(dicData))
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string strcontent = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(strcontent))
                        {
                            JObject jobj = JObject.Parse(strcontent);
                            if (!string.IsNullOrEmpty(jobj["id_token"] + ""))
                            {
                                return jobj["id_token"] + "";
                            }
                        }
                    }
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
        }
    }
    public sealed record UserGGInfo(string Email, string Name, string Thumbnail);
}
