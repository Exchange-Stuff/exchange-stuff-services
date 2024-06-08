using ExchangeStuff.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace ExchangeStuff.Core.Entities
{
    public class PermissionGroup:Auditable<Guid>
    {
        [MaxLength(10)]
        public string Name { get; set; }
        public bool IsActived { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Permission>? Permissions { get; set; }
    }
}
