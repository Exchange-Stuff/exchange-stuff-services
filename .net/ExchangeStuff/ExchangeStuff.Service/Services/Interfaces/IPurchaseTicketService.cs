using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.PurchaseTicket;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IPurchaseTicketService
    {
        Task<PurchaseTicketViewModel> CreatePurchaseTicket(CreatePurchaseTicketModel request);
        Task<PurchaseTicketViewModel> UpdatePurchaseTicket(UpdatePurchaseTicketModel request);
        Task<List<PurchaseTicketViewModel>> GetListPurchaseTicketByUserId(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!);
        Task<PurchaseTicketViewModel> GetPurchaseTicketDetail(Guid purchaseTicketId);
        Task<List<PurchaseTicketViewModel>> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!);
    }
}
