using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class User :Account
    {
        [MaxLength(10)]
        public string? StudentId { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }
         
        [MaxLength(12)]
        public string? Phone { get; set; }

        public GenderType? Gender { get; set; }

        public Guid? CampusId { get; set; }

        public Campus? Campus { get; set; }

        public UserBalance UserBalance { get; set; }

        public ICollection<FinancialTicket>? FinancialTickets { get; set; }
        public ICollection<PostTicket>? PostTickets { get; set; }
        public ICollection<PurchaseTicket>? PurchaseTickets { get; set; }

    }
}
