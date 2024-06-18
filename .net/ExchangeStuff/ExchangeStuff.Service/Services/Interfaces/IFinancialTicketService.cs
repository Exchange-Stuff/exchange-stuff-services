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
        Task<bool> CreateFinancialTicket(CreateFinancialTicketModel request);
        Task<bool> UpdateFinancialTicket(UpdateFinancialTicketModel request);
        Task<List<FinancialTicketViewModel>> GetFinancialTicketByUserId( int pageSize, int pageIndex, FinancialTicketStatus? status = null!);
        Task<FinancialTicketViewModel> GetFinancialTicketDetail(Guid financialTicketId);
        Task<List<FinancialTicketViewModel>> GetAllFinancialTicket(int pageSize, int pageIndex, FinancialTicketStatus? status = null!);

    }
}
