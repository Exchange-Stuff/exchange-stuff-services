using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.BanReasons
{
    public class BanReasonUpdateModel : BaseEntity<Guid>
    {
        [MaxLength(100)]
        public string Content { get; set; }
        public BanReasonType BanReasonType { get; set; }
    }
}
