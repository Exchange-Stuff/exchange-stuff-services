using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly ExchangeStuffContext _context;

        public AccountRepository(ExchangeStuffContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return null!;
            return (await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email))!;
        }
    }
}
