using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.BanReasons;
using ExchangeStuff.Service.Models.Users;

namespace ExchangeStuff.Service.Models.UserBanReports
{
    public class UserBanReportViewModel : Auditable<Guid>
    {
        public UserViewModel User { get; set; }

        public BanReasonViewModel BanReason { get; set; }

        public bool IsApproved { get; set; }
    }
}


