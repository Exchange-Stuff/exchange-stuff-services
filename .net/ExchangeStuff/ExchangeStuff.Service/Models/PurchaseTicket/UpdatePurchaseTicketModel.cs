using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Service.Models.PurchaseTicket
{
    public class UpdatePurchaseTicketModel : Auditable<Guid>
    {
        public PurchaseTicketStatus Status { get; set; }
    }
}
