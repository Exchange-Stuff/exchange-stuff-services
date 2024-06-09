using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.FinancialTickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IFinancialTicketService
    {
        Task<FinancialTicketViewModel> CreateFinancialTicket(CreateFinancialTicketModel request);
        Task<FinancialTicketViewModel> UpdateFinancialTicket(UpdateFinancialTicketModel request);
        Task<List<FinancialTicketViewModel>> GetFinancialTicketByUserId(Guid userId, int pageSize, int pageIndex, FinancialTicketStatus status);
        Task<FinancialTicketViewModel> GetFinancialTicketDetail(Guid financialTicketId);
        Task<List<FinancialTicketViewModel>> GetAllFinancialTicket(int pageSize, int pageIndex, FinancialTicketStatus status);

    }
}
