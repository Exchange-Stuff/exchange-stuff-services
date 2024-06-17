using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Service.Models.PurchaseTicket
{
    public class UpdatePurchaseTicketModel : BaseEntity<Guid>
    {
        public PurchaseTicketStatus Status { get; set; }
    }
}
