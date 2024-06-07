using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
