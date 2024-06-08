using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly DbSet<Account> _acccount;
        public AccountRepository(ExchangeStuffContext context) : base(context)
        {
            _acccount = context.Set<Account>();
        }
    }
}
