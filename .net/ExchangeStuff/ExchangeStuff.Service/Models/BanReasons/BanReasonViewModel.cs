using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;

namespace ExchangeStuff.Service.Models.BanReasons
{
    public class BanReasonViewModel : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public BanReasonType BanReasonType { get; set; }
    }
}
