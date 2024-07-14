using AutoMapper;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Repository.Repositories;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Moderators;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace ExchangeStuff.Service.Services.Impls
{
    public class AccountService : TokenService, IAccountService
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
        private readonly IIdentityUser<Guid> _identityUser;
        public AccountService(IConfiguration configuration, IUnitOfWork unitOfWork, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor, IIdentityUser<Guid> identityUser) : base(unitOfWork, configuration, httpContextAccessor, distributedCache, connectionMultiplexer, identityUser)
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
        }

        public async Task<AccountViewModel> GetAccount(Guid id, bool? includeBan = null!)
        {
            if (includeBan.HasValue && includeBan.Value)
            {
                return AutoMapperConfig.Mapper.Map<AccountViewModel>(await _accountRepository.GetOneAsync(x => x.Id == id, "PermissionGroups"));
            }
            return AutoMapperConfig.Mapper.Map<AccountViewModel>(await _accountRepository.GetOneAsync(x => x.Id == id && x.IsActived, "PermissionGroups"));
        }

        public async Task<PaginationItem<AccountViewModel>> GetAccounts(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null!)
        {
            var accounts = AutoMapperConfig.Mapper.Map<List<AccountViewModel>>
                (await _accountRepository.GetAccountsFilter(name, email, username, "PermissionGroups", includeBan, pageIndex,pageSize));
            return PaginationItem<AccountViewModel>.ToPagedList(accounts, pageIndex, pageSize);
        }

        public async Task<ModeratorViewModel> GetModerator(Guid id, bool? includeBan = null)
        {
            if (includeBan.HasValue && includeBan.Value)
            {
                return AutoMapperConfig.Mapper.Map<ModeratorViewModel>(await _moderatorRepository.GetOneAsync(x => x.Id == id, "PermissionGroups"));
            }
            return AutoMapperConfig.Mapper.Map<ModeratorViewModel>(await _moderatorRepository.GetOneAsync(x => x.Id == id && x.IsActived, "PermissionGroups"));
        }

        public async Task<PaginationItem<ModeratorViewModel>> GetModerators(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null)
        {
            var moderators = AutoMapperConfig.Mapper.Map<List<ModeratorViewModel>>
                (await _moderatorRepository.GetModeratorsFilter(name, email, username, "PermissionGroups", includeBan, pageIndex, pageSize));
            return PaginationItem<ModeratorViewModel>.ToPagedList(moderators, pageIndex, pageSize);
        }

        public async Task<UserViewModel> GetUser(Guid id, bool? includeBan = null!)
        {
            if (includeBan.HasValue && includeBan.Value)
            {
                return AutoMapperConfig.Mapper.Map<UserViewModel>(await _userRepository.GetOneAsync(x => x.Id == id, "PermissionGroups,Campus,UserBalance"));
            }
            return AutoMapperConfig.Mapper.Map<UserViewModel>(await _userRepository.GetOneAsync(x => x.Id == id && x.IsActived, "PermissionGroups,Campus,UserBalance"));
        }

        public async Task<PaginationItem<UserViewModel>> GetUsers(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null!)
        {
            var userVm = AutoMapperConfig.Mapper.Map<List<UserViewModel>>
                (await _userRepository.GetUserFilter(name, email, username, "PermissionGroups,Campus,UserBalance", includeBan));
            return PaginationItem<UserViewModel>.ToPagedList(userVm, pageIndex, pageSize);
        }

        public async Task<ModeratorViewModel> UpdateModerator(ModeratorUpdateModel moderatorUpdateModel)
        {
            if (moderatorUpdateModel.Id != _identityUser.AccountId) throw new Exception("You do not have permission");
            var moderator = await _moderatorRepository.GetOneAsync(x => x.Id == moderatorUpdateModel.Id && x.IsActived, forUpdate: true);
            if (moderator == null!) throw new Exception("Not found this user");
            moderator.Email = moderatorUpdateModel.Email ?? moderator.Email;
            moderator.Name = moderatorUpdateModel.Name ?? moderator.Name;
            moderator.Thumbnail = moderatorUpdateModel.Thumbnail ?? moderator.Thumbnail;
            bool passwordChanged = false;
            if (moderatorUpdateModel.Password != null)
            {
                string oldPassword = moderator.Password + "";
                if (oldPassword != "" && oldPassword == HashPassword(moderatorUpdateModel.Password)) throw new Exception("New password same old password");
                moderator.Password = HashPassword(moderatorUpdateModel.Password);
                passwordChanged = true;
            }
            _moderatorRepository.Update(moderator);
            await _uow.SaveChangeAsync();
            if (passwordChanged)
            {
                await InvalidAllSession(moderatorUpdateModel.Id);
            }
            return AutoMapperConfig.Mapper.Map<ModeratorViewModel>(moderator);
        }

        public async Task<UserViewModel> UpdateUser(UserUpdateModel userUpdateModel)
        {
           if (userUpdateModel.Id != _identityUser.AccountId) throw new Exception("You do not have permission");
            var user = await _userRepository.GetOneAsync(x => x.Id == userUpdateModel.Id && x.IsActived, forUpdate: true);
            if (user == null!) throw new Exception("Not found this user");

            user.Name = userUpdateModel.Name ?? user.Name;
            user.StudentId = userUpdateModel.StudentId ?? user.StudentId;
            user.Address = userUpdateModel.Address ?? user.Address;
            user.Thumbnail = userUpdateModel.Thumbnail ?? user.Thumbnail;
            user.Phone = userUpdateModel.Phone ?? user.Phone;
            user.Gender = userUpdateModel.Gender ?? user.Gender;

            bool passwordChanged = false;
            if (userUpdateModel.Password != null)
            {
                string oldPassword = user.Password + "";
                if (oldPassword != "" && oldPassword == HashPassword(userUpdateModel.Password))
                {
                    throw new Exception("New password same old password");
                }
                passwordChanged = true;
                user.Password = HashPassword(userUpdateModel.Password);
            }
            _userRepository.Update(user);
            await _uow.SaveChangeAsync();
            if (passwordChanged)
            {
                await InvalidAllSession(userUpdateModel.Id);
            }
            return AutoMapperConfig.Mapper.Map<UserViewModel>(user);
        }


    }
}
