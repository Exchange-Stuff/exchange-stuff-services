using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository.Repositories
{
    public class PurchaseTicketRepository : Repository<PurchaseTicket>, IPurchaseTicketRepository
    {
        private readonly DbSet<PurchaseTicket> _purchaseTicketRepository;

        public PurchaseTicketRepository(ExchangeStuffContext context) : base(context)
        {
            _purchaseTicketRepository = context.Set<PurchaseTicket>();
        }
    }
}
