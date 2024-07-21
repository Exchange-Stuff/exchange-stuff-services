using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.Products;

namespace ExchangeStuff.Service.Models.PurchaseTicket
{
    public class UpdatePurchaseTicketModel : BaseEntity<Guid>
    {
        public PurchaseTicketStatus Status { get; set; }
    }
}
