using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class FinancialTicket:Auditable<Guid>
    {
        public double Amount { get; set; }
        public Guid UserId { get; set; }
        public bool IsCredit { get; set; }
        public bool IsAccepted { get; set; }
        public User User { get; set; }
    }
}
