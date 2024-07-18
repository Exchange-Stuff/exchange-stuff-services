using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.FinancialTickets;
using ExchangeStuff.Service.Paginations;
namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IFinancialTicketService
    {
        Task<bool> CreateFinancialTicket(CreateFinancialTicketModel request);
        Task<bool> UpdateFinancialTicket(UpdateFinancialTicketModel request);
        Task<PaginationItem<FinancialTicketViewModel>> GetFinancialTicketByUserId( int pageSize, int pageIndex, FinancialTicketStatus? status = null!);
        Task<FinancialTicketViewModel> GetFinancialTicketDetail(Guid financialTicketId);
        Task<PaginationItem<FinancialTicketViewModel>> GetAllFinancialTicket(int pageSize, int pageIndex, FinancialTicketStatus? status = null!);
        Task<PaginationItem<FinancialTicketViewModel>> GetAllFilter(int pageSize, int pageIndex, DateTime? from, DateTime? to, FinancialTicketStatus? status, int? sort);

    }
}
