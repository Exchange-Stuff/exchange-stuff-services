using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories.Base;

namespace ExchangeStuff.Core.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetAccountByEmail(string email);
        Task<List<Account>> GetAccountsFilter(string? name = null!, string? email = null!, string? username = null!, string? includes = null!, int? pageIndex = null!, int? pageSize = null!, bool? includeBan=null!);
    }
}
