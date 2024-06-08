using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;

namespace ExchangeStuff.Repository.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly ExchangeStuffContext _context;

        public PermissionRepository(ExchangeStuffContext context) : base(context)
        {
            _context=context;
        }

    }
}
