using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Dashboard;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Impls;

public class DashboardService : IDashboardService
{
    private readonly IPurchaseTicketRepository _purchaseTicketRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DateTime firstDayInWeek;
    private readonly DateTime lastDayInWeek;
    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _purchaseTicketRepo = _unitOfWork.PurchaseTicketRepository;
        firstDayInWeek = GetStartOfWeek(DateTime.Now, DayOfWeek.Monday);
        lastDayInWeek = firstDayInWeek.AddDays(7);
    }
    private static DateTime GetStartOfWeek(DateTime date, DayOfWeek startOfWeek)
    {
        var todayNumber = date.DayOfWeek;
        var addDay = todayNumber - startOfWeek;
        addDay = addDay < 0 ? 6 : addDay;
        return date.AddDays(-addDay).Date;
    }

    public async Task<List<PurchaseTicketViewModel>> GetListPurchaseThisWeek()
    {
        var listTicket = await _purchaseTicketRepo.GetManyAsync(predicate: p => p.CreatedOn >= firstDayInWeek && p.CreatedOn < lastDayInWeek);
        return AutoMapperConfig.Mapper.Map<List<PurchaseTicketViewModel>>(listTicket);
    }

    public async Task<ReportPurchaseTicket> GetReportPurchaseTicketThisWeek()
    {
        var listTicket = await _purchaseTicketRepo.GetManyAsync(predicate: p => p.CreatedOn >= firstDayInWeek && p.CreatedOn < lastDayInWeek);
        var totalTicket = listTicket.Count;
        var totalCancelled = listTicket.Where(p => p.Status == Core.Enums.PurchaseTicketStatus.Cancelled).Count();
        var totalConfirmed = listTicket.Where(p => p.Status == Core.Enums.PurchaseTicketStatus.Confirmed).Count();

        var listLastWeek = await _purchaseTicketRepo.GetManyAsync(predicate: p => p.CreatedOn >= firstDayInWeek.AddDays(-7) && p.CreatedOn < firstDayInWeek);
        var totalTicketLastWeek = listLastWeek.Count;
        var totalCancelledLastWeek = listLastWeek.Where(p => p.Status == Core.Enums.PurchaseTicketStatus.Cancelled).Count();
        var totalConfirmedLastWeek = listLastWeek.Where(p => p.Status == Core.Enums.PurchaseTicketStatus.Confirmed).Count();

        double percentTotalWithLastWeek = 0;
        if (totalTicket == 0 && totalTicketLastWeek == 0)
        {
            percentTotalWithLastWeek = 0;
        }
        else if (totalTicket == 0)
        {
            percentTotalWithLastWeek = -100;
        }
        else if (totalTicketLastWeek == 0)
        {
            percentTotalWithLastWeek = 100;
        }
        else
        {
            percentTotalWithLastWeek = ((totalTicket - totalTicketLastWeek) / (double)totalTicketLastWeek) * 100;
        }

        double percentTotalCancelledLastWeek = 0;
        if (totalCancelled == 0 && totalCancelledLastWeek == 0)
        {
            percentTotalCancelledLastWeek = 0;
        }
        else if (totalCancelledLastWeek == 0)
        {
            percentTotalCancelledLastWeek = 100;
        }
        else if (totalCancelled == 0)
        {
            percentTotalCancelledLastWeek = -100;
        }
        else
        {
            percentTotalCancelledLastWeek = ((totalCancelled - totalCancelledLastWeek) / (double)totalCancelledLastWeek) * 100;
        }

        double percentTotalConfirmedLastWeek = 0;
        if (totalConfirmed == 0 && totalCancelledLastWeek == 0)
        {
            percentTotalConfirmedLastWeek = 0;
        }
        else if (totalConfirmedLastWeek == 0)
        {
            percentTotalConfirmedLastWeek = 100;
        }
        else if (totalConfirmed == 0)
        {
            percentTotalConfirmedLastWeek = -100;
        }
        else
        {
            percentTotalConfirmedLastWeek = ((totalConfirmed - totalConfirmedLastWeek) / (double)totalTicketLastWeek) * 100;
        }

        ReportPurchaseTicket report = new ReportPurchaseTicket
        {
            TotalTicket = totalTicket,
            PercentTotalWithLastWeek = percentTotalWithLastWeek,
            TotalCancelled = totalCancelled,
            PercentCancelledWithLastWeek = percentTotalCancelledLastWeek,
            TotalConfirmed = totalConfirmed,
            PercentConfirmedWithLastWeek = percentTotalConfirmedLastWeek,
        };
        return report;
    }
}
