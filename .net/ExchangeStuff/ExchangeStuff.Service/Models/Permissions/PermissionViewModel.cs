using ExchangeStuff.Service.Models.PermissionGroups;
using ExchangeStuff.Service.Models.Resources;

namespace ExchangeStuff.Service.Models.Permissions
{
    public class PermissionViewModel
    {
        public int PermissionValue { get; set; }
        public PermisisonGroupViewModel PermissionGroup { get; set; }
        public ResourceViewModel Resource { get; set; }
    }
}
