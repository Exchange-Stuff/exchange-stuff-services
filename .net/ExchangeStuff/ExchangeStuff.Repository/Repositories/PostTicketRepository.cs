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
    public class PostTicketRepository : Repository<PostTicket>, IPostTicketRepository
    {
        public PostTicketRepository(ExchangeStuffContext context) : base(context) 
        {
            
        }
    }
}
