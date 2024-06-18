using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class UserBanReport : Auditable<Guid>
    {
        public Guid UserId { get; set; }

        public Guid BanReasonId { get; set; }

        public User User { get; set; }

        public BanReason BanReason { get; set; }

        public bool IsApproved { get; set; }

    }
}
