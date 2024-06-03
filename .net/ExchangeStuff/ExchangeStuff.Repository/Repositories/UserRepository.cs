using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(ExchangeStuffContext context) : base(context)
        {
            _users=context.Set<User>();
        }
    }
}
