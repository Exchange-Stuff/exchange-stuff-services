namespace ExchangeStuff.Service.Models.UserBanReports
{
    public class UserBanReportCreateModel
    {
        public Guid UserId { get; set; }

        public Guid BanReasonId { get; set; }

    }
}
