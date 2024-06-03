using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class Resource:Auditable<Guid>
    {
        public string Name { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
