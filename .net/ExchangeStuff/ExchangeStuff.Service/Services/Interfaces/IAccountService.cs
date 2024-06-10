using ExchangeStuff.Service.Models.Users;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<UserViewModel>> GetUsers(string? name = null!, string? email = null!, string? username = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<UserViewModel> GetUser(Guid id);
    }
}
