using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;

namespace ExchangeStuff.Repository.Repositories
{
    public class UserBanReportRepository : Repository<UserBanReport>, IUserBanReportRepository
    {
        public UserBanReportRepository(ExchangeStuffContext context) : base(context)
        {
        }
    }
}
