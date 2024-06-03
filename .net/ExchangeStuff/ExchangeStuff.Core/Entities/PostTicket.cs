using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Core.Entities
{
    public class PostTicket:Auditable<Guid>
    {
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public PostTicketStatus Status { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
