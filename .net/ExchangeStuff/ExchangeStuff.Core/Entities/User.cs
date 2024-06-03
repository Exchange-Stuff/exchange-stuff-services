using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class User : Auditable<Guid>
    {
        [MaxLength(30)]
        public string Username { get; set; }

        [MaxLength(10)]
        public string StudentId { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [MaxLength(12)]
        public string? Phone { get; set; }

        public GenderType? Gender { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public string Password { get; set; }

        public string? Thumbnail { get; set; }

        public Guid CampusId { get; set; }

        public Guid RoleId { get; set; }

        public bool IsActived { get; set; }

        public Campus Campus { get; set; }
        public UserBalance  UserBalance { get; set; }
        public ICollection<PermissionGroup> PermissionGroups { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<FinancialTicket>? FinancialTickets { get; set; }
        public ICollection<PostTicket>? PostTickets { get; set; }
        public ICollection<PurchaseTicket>? PurchaseTickets { get; set; }

    }
}
