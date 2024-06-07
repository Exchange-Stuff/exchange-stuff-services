using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.PurchaseTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IPurchaseTicketService
    {
        Task<ActionResult> CreatePurchaseTicket(CreatePurchaseTicketModel request);
        Task<ActionResult> UpdatePurchaseTicket()
    }
}
