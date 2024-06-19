using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Moderators;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IModeratorRepository _moderatorRepository;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _userRepository = _uow.UserRepository;
            _accountRepository = _uow.AccountRepository;
            _moderatorRepository = _uow.ModeratorRepository;
        }

        public async Task<AccountViewModel> GetAccount(Guid id, bool? includeBan = null!)
        {
            if (includeBan.HasValue && includeBan.Value)
            {
                return AutoMapperConfig.Mapper.Map<AccountViewModel>(await _accountRepository.GetOneAsync(x => x.Id == id, "PermissionGroups"));
            }
            return AutoMapperConfig.Mapper.Map<AccountViewModel>(await _accountRepository.GetOneAsync(x => x.Id == id && x.IsActived, "PermissionGroups"));
        }

        public async Task<List<AccountViewModel>> GetAccounts(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null!)
             => AutoMapperConfig.Mapper.Map<List<AccountViewModel>>(await _accountRepository.GetAccountsFilter(name, email, username, "PermissionGroups", pageIndex, pageSize, includeBan));

        public async Task<ModeratorViewModel> GetModerator(Guid id, bool? includeBan = null)
        {
            if (includeBan.HasValue && includeBan.Value)
            {
                return AutoMapperConfig.Mapper.Map<ModeratorViewModel>(await _moderatorRepository.GetOneAsync(x => x.Id == id, "PermissionGroups"));
            }
            return AutoMapperConfig.Mapper.Map<ModeratorViewModel>(await _moderatorRepository.GetOneAsync(x => x.Id == id && x.IsActived, "PermissionGroups"));
        }

        public async Task<List<ModeratorViewModel>> GetModerators(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null)
             => AutoMapperConfig.Mapper.Map<List<ModeratorViewModel>>(await _moderatorRepository.GetModeratorsFilter(name, email, username, "PermissionGroups", pageIndex, pageSize, includeBan));

        public async Task<UserViewModel> GetUser(Guid id, bool? includeBan = null!)
        {
            if (includeBan.HasValue && includeBan.Value)
            {
                return AutoMapperConfig.Mapper.Map<UserViewModel>(await _userRepository.GetOneAsync(x => x.Id == id, "PermissionGroups,Campus,UserBalance"));
            }
            return AutoMapperConfig.Mapper.Map<UserViewModel>(await _userRepository.GetOneAsync(x => x.Id == id && x.IsActived, "PermissionGroups,Campus,UserBalance"));
        }

        public async Task<List<UserViewModel>> GetUsers(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null!)
            => AutoMapperConfig.Mapper.Map<List<UserViewModel>>(await _userRepository.GetUserFilter(name, email, username, "PermissionGroups,Campus,UserBalance", pageIndex, pageSize, includeBan));
    }
}
