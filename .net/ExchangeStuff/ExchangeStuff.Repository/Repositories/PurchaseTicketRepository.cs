using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository.Repositories
{
    public class PurchaseTicketRepository : Repository<PurchaseTicket>, IPurchaseTicketRepository
    {

        public PurchaseTicketRepository(ExchangeStuffContext context) : base(context)
        {
        }
    }
}
