using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class Permission:BaseEntity<Guid>
    {
        public Guid PermissionGroupId { get; set; }
        public Guid ResourceId { get; set; }
        public int PermissionValue { get; set; }
        public PermissionGroup PermissionGroup { get; set; }
        public Resource Resource { get; set; }
    }
}
