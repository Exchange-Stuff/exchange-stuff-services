using ExchangeStuff.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class Payment : Auditable<Guid>
    {
        public double Amount { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
    }
}
