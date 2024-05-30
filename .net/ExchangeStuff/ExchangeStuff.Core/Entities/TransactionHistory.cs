using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Core.Entities
{
    public class TransactionHistory : Auditable<Guid>
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public bool IsCredit { get; set; }
        public TransactionType TransactionType { get; set; }
        public User User { get; set; }
    }
}
