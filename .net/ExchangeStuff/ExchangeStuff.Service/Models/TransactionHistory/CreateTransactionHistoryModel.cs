using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Service.Models.TransactionHistory
{
    public class CreateTransactionHistoryModel
    {
        public double Amount { get; set; }
        public bool IsCredit { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
