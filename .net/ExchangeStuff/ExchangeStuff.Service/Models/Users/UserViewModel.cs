using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.Campuses;
using ExchangeStuff.Service.Models.PermissionGroups;

namespace ExchangeStuff.Service.Models.Users
{
    public class UserViewModel : BaseEntity<Guid>
    {
        public string? Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? Thumbnail { get; set; }

        public List<PermisisonGroupViewModel> PermissionGroups { get; set; }

        public string? StudentId { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public GenderType? Gender { get; set; }

        public Guid? CampusId { get; set; }

        public CampusViewModel? Campus { get; set; }

        public UserBalanceViewModel UserBalance { get; set; }
    }
}
