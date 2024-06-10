using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Service.Models.TransactionHistory
{
    public class TransactionHistoryViewModel : Auditable<Guid>
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public bool IsCredit { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
