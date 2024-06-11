using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.DTOs
{
    public class PermissionGroupDTO : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
