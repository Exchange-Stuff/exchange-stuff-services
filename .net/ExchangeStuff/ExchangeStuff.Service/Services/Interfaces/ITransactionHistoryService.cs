using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.TransactionHistory;
using ExchangeStuff.Service.Paginations;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface ITransactionHistoryService
    {
        Task<bool> CreateTransactionHistory(CreateTransactionHistoryModel request);
        Task<PaginationItem<TransactionHistoryViewModel>> GetAllTransactionHistory(int pageSize, int pageIndex, TransactionType? type = null);
        Task<PaginationItem<TransactionHistoryViewModel>> GetListTransactionHistoryByUserId(int pageSize, int pageIndex);
        Task<TransactionHistoryViewModel> GetTransactionHistoryDetail(Guid transactionHistoryId);
    }
}
