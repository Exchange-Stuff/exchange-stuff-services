using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class BanReason : BaseEntity<Guid>
    {
        [MaxLength(100)]
        public string Content { get; set; }
        public BanReasonType BanReasonType { get; set; }

        public ICollection<UserBanReport>? UserBanReports { get; set; }
        public ICollection<ProductBanReport>? ProductBanReports { get; set; }
    }
}
