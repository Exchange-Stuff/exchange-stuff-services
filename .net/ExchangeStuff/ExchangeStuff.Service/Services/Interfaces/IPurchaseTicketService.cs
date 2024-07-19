using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Paginations;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IPurchaseTicketService
    {
        Task<bool> CreatePurchaseTicket(CreatePurchaseTicketModel request);
        Task<bool> UpdatePurchaseTicket(UpdatePurchaseTicketModel request);
        Task<PaginationItem<PurchaseTicketViewModel>> GetListPurchaseTicketByUserId(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!);
        Task<PurchaseTicketViewModel> GetPurchaseTicketDetail(Guid purchaseTicketId);
        Task<PaginationItem<PurchaseTicketViewModel>> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!);
    }
}
