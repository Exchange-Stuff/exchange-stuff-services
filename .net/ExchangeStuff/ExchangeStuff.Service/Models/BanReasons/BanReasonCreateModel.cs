using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Service.Models.BanReasons
{
    public class BanReasonCreateModel
    {
        [MaxLength(100)]
        public string Content { get; set; }

        public BanReasonType BanReasonType { get; set; }
    }
}
