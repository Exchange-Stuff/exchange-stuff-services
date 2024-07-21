using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.FinancialTickets;
namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IFinancialTicketService
    {
        Task<bool> CreateFinancialTicket(CreateFinancialTicketModel request);
        Task<bool> UpdateFinancialTicket(UpdateFinancialTicketModel request);
        Task<List<FinancialTicketViewModel>> GetFinancialTicketByUserId( int pageSize, int pageIndex, FinancialTicketStatus? status = null!);
        Task<FinancialTicketViewModel> GetFinancialTicketDetail(Guid financialTicketId);
        Task<List<FinancialTicketViewModel>> GetAllFinancialTicket(int pageSize, int pageIndex, FinancialTicketStatus? status = null!);
        Task<List<FinancialTicketViewModel>> GetAllFilter(int pageSize, int pageIndex, DateTime? from, DateTime? to, FinancialTicketStatus? status, int? sort);

    }
}
