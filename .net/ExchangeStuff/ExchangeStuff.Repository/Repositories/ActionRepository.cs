using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;

namespace ExchangeStuff.Repository.Repositories
{
    public class ActionRepository : Repository<Core.Entities.Action>, IActionRepository
    {
        public ActionRepository(ExchangeStuffContext context) : base(context)
        {
        }
    }
}
