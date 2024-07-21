using ExchangeStuff.Service.Models.Dashboard;
using ExchangeStuff.Service.Models.PurchaseTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Interfaces;

public interface IDashboardService
{
    Task<ReportPurchaseTicket> GetReportPurchaseTicketThisWeek();
    Task<List<PurchaseTicketViewModel>> GetListPurchaseThisWeek();

}
