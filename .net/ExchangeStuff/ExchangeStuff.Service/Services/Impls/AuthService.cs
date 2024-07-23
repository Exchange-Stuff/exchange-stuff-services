using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Repository.Repositories;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Moderators;
using ExchangeStuff.Service.Models.Tokens;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
        private readonly IModeratorRepository _moderatorRepository;
        private readonly IPermissionGroupRepository _permissionGroupRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IActionRepository _actionRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IIdentityUser<Guid> _identityUser;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor, IIdentityUser<Guid> identityUser) : base(unitOfWork, configuration, httpContextAccessor, distributedCache, connectionMultiplexer, identityUser)
        {
            _identityUser = identityUser;
            _uow = unitOfWork;
            _httpConextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
            _configuration = configuration;
            _configuration.GetSection(nameof(GoogleAuthDTO)).Bind(_googleAuthDTO);
            _userRepository = _uow.UserRepository;
            _accountRepository = _uow.AccountRepository;
            _moderatorRepository = _uow.ModeratorRepository;
            _permissionGroupRepository = _uow.PermissionGroupRepository;
            _resourceRepository = _uow.ResourceRepository;
            _permissionRepository = _uow.PermissionRepository;
            _actionRepository = _uow.ActionRepository;
            _adminRepository = _uow.AdminRepository;
            _tokenRepository = _uow.TokenRepository;
        }

        public async Task<bool> ValidScreen(string resource)
        {
            if (string.IsNullOrEmpty(resource)) return false;

            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Access denial");

            var rsrc = await _resourceRepository.GetOneAsync(x => x.Name == resource);
            if (rsrc == null) throw new UnauthorizedAccessException("Access denial");

            var acc = await _accountRepository.GetOneAsync(x => x.Id == claim.Id, "PermissionGroups");
            if (acc == null) throw new UnauthorizedAccessException("Access denial");
            var permissiongids = acc.PermissionGroups.Select(x => x.Id);

            if (permissiongids.Any() is false) return false;
            var permissionGroups = await _permissionGroupRepository.GetManyAsync(x => permissiongids.Contains(x.Id));

            if (permissionGroups.Any() is false) return false;
            var permissionId1 = permissionGroups.Select(x => x.Id);

            var permissions = await _permissionRepository.GetManyAsync(x => permissionId1.Contains(x.PermissionGroupId) && x.ResourceId == rsrc.Id);
            if (permissions.Any() is false) return false;

            var actions = await _actionRepository.GetManyAsync();
            if (actions.Any() is false) throw new Exception("No action in system");
            if (permissions != null && permissions.Count > 0)
            {
                bool access = false;
                foreach (var item in permissions)
                {
                    access = ValidActionResource(actions, "Access", item.PermissionValue);
                    if (!access) throw new UnauthorizedAccessException("Access denial");
                }
                return access;
            }
            return false;
        }

        private bool ValidActionResource(List<ExchangeStuff.Core.Entities.Action> actions, string action, int roleValue)
        {
            if (actions?.Any() != true) return false;

            actions = actions.OrderBy(x => x.Index).ToList();

            char[] authorizeString = ReverseString(DecimalToBinary(roleValue)).ToArray();

            Dictionary<string, bool> actionKey = new Dictionary<string, bool>();
            int i = 0;
            foreach (var item in authorizeString)
            {
                bool vlue;
                if (item == '0')
                {
                    vlue = false;
                }
                else if (item == '1')
                {
                    vlue = true;
                }
                else
                {
                    return false;
                }
                actionKey.Add(actions[i].Name.ToLower(), vlue);
                i++;
            }
            if (actionKey.ContainsKey(action.ToLower()))
            {
                return actionKey[action.ToLower()];
            }
            return false;
        }

        private string DecimalToBinary(int decimalNumber)
        {
            if (decimalNumber == 0)
                return "0";

            StringBuilder binary = new StringBuilder();

            while (decimalNumber > 0)
            {
                binary.Insert(0, decimalNumber % 2);
                decimalNumber /= 2;
            }

            return binary.ToString();
        }

        private char[] ReverseString(string str)
        {
            char[] chars = str.ToCharArray();
            int length = str.Length;
            for (int i = 0; i < length / 2; i++)
            {
                char temp = chars[i];
                chars[i] = chars[length - 1 - i];
                chars[length - 1 - i] = temp;
            }
            return chars;
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

        public async Task<TokenViewModel> GetTokenAdmin(string param)
        {
            if (param != "")
            {
                param = param.Substring(1);
                string[] splitParam = param.Split('&', StringSplitOptions.RemoveEmptyEntries);
                string code = "";
                bool isFpt = false;
                bool finded = false;
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
                        isFpt = false;
                    }
                }
                if (!isFpt)
                {
                    throw new UnauthorizedAccessException("Not allow this email");
                }
                if (!string.IsNullOrEmpty(code.Trim()))
                {
                    var token = await GetAccessTokenAsync(code);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var userindor = GetAuth(token);
                        if (userindor == null!) throw new UnauthorizedAccessException("UserInfor invalid");
                        var acc = await AdminSignin(userindor);
                        if (acc == null) throw new UnauthorizedAccessException("Not found user");
                        var tk = await _tokenRepository.GetManyAsync(x => x.AccountId == acc.Id, forUpdate: true);
                        if (tk.Any())
                        {
                            foreach (var item in tk)
                            {
                                _tokenRepository.Remove(item);
                                await DeleteAccessToken(item.AccessToken);
                            }
                            await _uow.SaveChangeAsync();
                            throw new UnauthorizedAccessException("Login session expired, another device online try again");
                        }
                        var tks = await GenerateToken(await _adminRepository.GetOneAsync(x => x.Id == acc.Id, forUpdate: true));
                        await SavePermissionGroupAdmin(acc.Id);
                        var ntoken = await SaveAccessToken(tks, acc.Id);
                        return AutoMapperConfig.Mapper.Map<TokenViewModel>(ntoken);
                    }
                }
                throw new UnauthorizedAccessException("Not found auth code");
            }
            return null!;
        }

        public async Task<Admin> AdminSignin(UserGGInfo userinfo)
        {
            if (userinfo != null!)
            {
                Admin admin = await _adminRepository.GetOneAsync(x => x.Email == userinfo.Email);
                if (admin != null!)
                {
                    admin.Thumbnail = userinfo.Thumbnail != admin.Thumbnail ? userinfo.Thumbnail : admin.Thumbnail;
                    admin.Name = userinfo.Name != admin.Name ? userinfo.Name : admin.Name;
                    await _uow.SaveChangeAsync();
                    return admin;
                }
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
                        IsActived = true,
                        Username = userinfo.Email
                    };
                    UserBalance userBalance = new UserBalance
                    {
                        Balance = 0,
                        UserId = user.Id
                    };
                    user.UserBalance = userBalance;
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
                    if (!account.IsActived) throw new UnauthorizedAccessException();
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
                    string strcontenst = await response.Content.ReadAsStringAsync();

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

        public async Task<ModeratorViewModel> CreateModerator(ModeratorCreateModel moderatorCreateModel)
        {
            if (moderatorCreateModel.Username.Split(" ").Length > 0) throw new Exception("Username not allow [space]");
            var mopderatorCheck = await _moderatorRepository.GetOneAsync(x => x.Username == moderatorCreateModel.Username);
            if (mopderatorCheck != null!) throw new Exception("Moderator username already exist");
            Moderator moderator = new Moderator
            {
                Username = moderatorCreateModel.Username,
                Name = moderatorCreateModel.Name,
                Password = HashPassword(moderatorCreateModel.Password),
                IsActived = true
            };

            var permissionGroups = await _permissionGroupRepository.GetManyAsync(x => x.Name == GroupPermission.MODERATOR, forUpdate: true);
            if (permissionGroups.Any() is false) throw new Exception("No permission group in system");
            moderator.PermissionGroups = permissionGroups;

            await _moderatorRepository.AddAsync(moderator);
            var rs = await _uow.SaveChangeAsync();
            return rs > 0 ? AutoMapperConfig.Mapper.Map<ModeratorViewModel>(moderator) : throw new Exception("Can't create new moderator");
        }

        public async Task<TokenViewModel> LoginUsernameAndPwd(LoginRd loginRd)
        {
            var newpwd = HashPassword("string");
            if (string.IsNullOrEmpty(loginRd.Username) || string.IsNullOrEmpty(loginRd.Password)) throw new Exception("Username and password required");
            Account account = null!;
            account = await _accountRepository.GetOneAsync(x => x.Email != null && x.Email == loginRd.Username && x.Password == HashPassword(loginRd.Password) && x.IsActived);
            if (account == null)
            {
                account = await _accountRepository.GetOneAsync(x => x.Username != null && x.Username == loginRd.Username && x.Password == HashPassword(loginRd.Password) && x.IsActived);
            }
            if (account == null!) throw new Exception("Wrong username or password");
            var tokenSystem = await GenerateToken(account);

            await SavePermissionGroup(account.Id);
            var ntoken = await SaveAccessToken(tokenSystem, account.Id);
            return AutoMapperConfig.Mapper.Map<TokenViewModel>(ntoken);
        }

        public async Task<bool> DeleteAccount(Guid id)
        {
            if (id == _identityUser.AccountId) throw new Exception("You can't delete yourself");
            var account = await _accountRepository.GetOneAsync(x => x.Id == id, forUpdate: true, include: "PermissionGroups");
            if (account == null!) throw new Exception("Not found this account");
            var permissionTargets = account.PermissionGroups.Where(x => x.Name == GroupPermission.ADMIN);
            var accCurrent = await _accountRepository.GetOneAsync(x => x.Id == _identityUser.AccountId, "PermissionGroups");
            if (accCurrent == null!) throw new UnauthorizedAccessException("Login session expired");
            var permissionGroupIds = accCurrent.PermissionGroups.Select(x => x.Id);
            var permissionGroupUser = await _permissionGroupRepository.GetManyAsync(x => permissionGroupIds.Contains(x.Id));
            if (permissionTargets.Any() is false)
            {
                List<PermissionGroup> permissionU = permissionGroupUser.Where(x => x.Name == GroupPermission.DEFAULT).ToList();
                if (permissionU.Any() && permissionGroupUser.Count == 1) throw new Exception("You do not have permission");

                account.IsActived = false;
            }
            else
            {
                List<PermissionGroup> permissionU = permissionGroupUser.Where(x => x.Name == GroupPermission.ADMIN).ToList();
                if (permissionU.Any())
                {
                    account.IsActived = false;
                }
                else
                {
                    throw new Exception("You do not have permission");
                }
            }
            await InvalidAllSession(id);
            await _uow.SaveChangeAsync();
            return true;
        }
    }

    public sealed record UserGGInfo(string Email, string Name, string Thumbnail);

    public sealed record LoginRd(string Username, string Password);
}
