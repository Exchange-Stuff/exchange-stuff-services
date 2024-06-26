using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Moderators;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Paginations;


namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IAccountService
    {
        Task<PaginationItem<UserViewModel>> GetUsers(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null!);
        Task<UserViewModel> GetUser(Guid id, bool? includeBan = null!);
        Task<PaginationItem<AccountViewModel>> GetAccounts(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null!);
        Task<AccountViewModel> GetAccount(Guid id, bool? includeBan = null!);
        Task<PaginationItem<ModeratorViewModel>> GetModerators(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null, bool? includeBan = null!);
        Task<ModeratorViewModel> GetModerator(Guid id, bool? includeBan = null!);
        Task<ModeratorViewModel> UpdateModerator(ModeratorUpdateModel moderatorUpdateModel);
        Task<UserViewModel> UpdateUser(UserUpdateModel userUpdateModel);
    }
}
