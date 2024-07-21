namespace ExchangeStuff.Service.Models.Dashboard;

public class ReportPurchaseTicket
{
    public int TotalCancelled { get; set; }
    public double PercentCancelledWithLastWeek { get; set; }
    public int TotalConfirmed { get; set; }
    public double PercentConfirmedWithLastWeek { get; set; }
    public int TotalTicket { get; set; }
    public double PercentTotalWithLastWeek { get; set; }
}
