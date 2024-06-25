using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ExchangeStuffContext _context;

        public UserRepository(ExchangeStuffContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUserFilter(string? name = null!, string? email = null!, string? username = null!, string? includes = null!,bool? includeBan = null!, int? pageIndex=null, int? pageSize=null!)
        {
            IQueryable<User> query = null!;
            if (includeBan.HasValue && includeBan.Value)
            {
                query = _context.Users;
            }
            else
            {
                query = _context.Users.Where(x => x.IsActived);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x => x.Email != null && x.Email.ToLower().Contains(email.ToLower()));
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
