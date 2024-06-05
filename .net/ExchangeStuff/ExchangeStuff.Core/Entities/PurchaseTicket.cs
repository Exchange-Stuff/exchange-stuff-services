using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class PurchaseTicket : Auditable<Guid>
    {
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        [MaxLength(10)]
        public string StudentId { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        [MaxLength(30)]
        public string CampusName { get; set; }
        public PurchaseTicketStatus Status { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
    }
}
