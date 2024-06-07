using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Impls
{
    public class PurchaseTicketService : IPurchaseTicketService
    {
        private readonly IIdentityUser<Guid> _identityUser;
    }
}
