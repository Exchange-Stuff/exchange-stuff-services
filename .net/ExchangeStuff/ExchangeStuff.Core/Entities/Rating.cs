using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeStuff.Core.Entities
{
    public class Rating
    {
        [Key, ForeignKey("PurchaseTicket")]
        public Guid PurchaseTicketId { get; set; }

        [MaxLength(100)]
        public string Content { get; set; }

        public EvaluateType EvaluateType { get; set; }

        public PurchaseTicket PurchaseTicket { get; set; }
    }
}
