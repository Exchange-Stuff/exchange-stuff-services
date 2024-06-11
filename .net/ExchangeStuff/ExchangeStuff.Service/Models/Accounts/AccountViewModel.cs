using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.PermissionGroups;

namespace ExchangeStuff.Service.Models.Accounts
{
    public class AccountViewModel : BaseEntity<Guid>
    {
        public string? Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? Thumbnail { get; set; }

        public List<PermisisonGroupViewModel> PermissionGroups { get; set; }
    }
}
