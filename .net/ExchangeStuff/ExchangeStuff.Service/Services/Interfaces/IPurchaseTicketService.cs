using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
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
        Task<ActionResult> UpdatePurchaseTicket(UpdatePurchaseTicketModel request);
        Task<ActionResult> GetListPurchaseTicketByUserId(Guid userId, int pageSize, int pageIndex, PurchaseTicketStatus? status = null!);
        Task<ActionResult> GetPurchaseTicketDetail(Guid purchaseTicketId);
        Task<ActionResult> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!);
    }
}
