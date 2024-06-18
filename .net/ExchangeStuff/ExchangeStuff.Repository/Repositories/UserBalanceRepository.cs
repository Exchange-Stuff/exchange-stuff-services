using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Repository.Repositories
{
    public class UserBalanceRepository : Repository<UserBalance>,IUserBalanceRepository
    {
        public UserBalanceRepository(ExchangeStuffContext context) : base(context)
        {
        }
    }
}
