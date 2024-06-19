using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories.Base;

namespace ExchangeStuff.Core.Repositories
{
    public interface IModeratorRepository:IRepository<Moderator>
    {
        Task<List<Account>> GetModeratorsFilter(string? name = null!, string? email = null!, string? username = null!, string? includes = null!, int? pageIndex = null!, int? pageSize = null!, bool? includeBan = null!);

    }
}
