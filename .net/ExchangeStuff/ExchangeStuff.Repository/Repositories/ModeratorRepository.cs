using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository.Repositories
{
    public class ModeratorRepository : Repository<Moderator>, IModeratorRepository
    {
        private readonly ExchangeStuffContext _context;

        public ModeratorRepository(ExchangeStuffContext context) : base(context)
        {
            _context=context;
        }
        public async Task<List<Account>> GetModeratorsFilter(string? name = null!, string? email = null!, string? username = null!, string? includes = null!, int? pageIndex = null!, int? pageSize = null!, bool? includeBan = null!)
        {
            IQueryable<Account> query = null!;
            if (includeBan.HasValue && includeBan.Value)
            {
                query = _context.Moderators;
            }
            else
            {
                query = _context.Moderators.Where(x => x.IsActived);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x => x.Email != null! && x.Email.ToLower().Contains(email.ToLower()));
            }
            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(x => x.Username != null && x.Username.ToLower().Contains(username.ToLower()));
            }
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var item in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item).AsNoTracking();
                }
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                query = query.Skip(index * size).Take(size);
            }
            return await query.ToListAsync();
        }
    }
}
