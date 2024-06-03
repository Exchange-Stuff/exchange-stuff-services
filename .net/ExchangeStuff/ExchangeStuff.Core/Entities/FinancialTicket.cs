using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Core.Entities
{
    public class FinancialTicket:Auditable<Guid>
    {
        public double Amount { get; set; }
        public Guid UserId { get; set; }
        public bool IsCredit { get; set; }
        public FinancialTicketStatus Status { get; set; }
        public User User { get; set; }
    }
}
