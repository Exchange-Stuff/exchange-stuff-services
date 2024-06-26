using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories.Base;

namespace ExchangeStuff.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUserFilter(string? name = null!, string? email = null!, string? username = null!, string? includes = null!, bool? includeBan = null!, int? pageIndex = null, int? pageSize = null!);
    }
}
