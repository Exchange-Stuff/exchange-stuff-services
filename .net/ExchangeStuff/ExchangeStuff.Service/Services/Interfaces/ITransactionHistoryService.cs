using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.TransactionHistory;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface ITransactionHistoryService
    {
        Task<bool> CreateTransactionHistory(CreateTransactionHistoryModel request);
        Task<List<TransactionHistoryViewModel>> GetAllTransactionHistory(int pageSize, int pageIndex, TransactionType? type = null);
        Task<List<TransactionHistoryViewModel>> GetListTransactionHistoryByUserId(int pageSize, int pageIndex, TransactionType? type = null!);
        Task<TransactionHistoryViewModel> GetTransactionHistoryDetail(Guid transactionHistoryId);
    }
}
