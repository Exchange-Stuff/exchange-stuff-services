using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Service.Models.TransactionHistory
{
    public class TransactionHistoryViewModel : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public bool IsCredit { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
