using ExchangeStuff.Core.Common;
using ExchangeStuff.Service.Models.PermissionGroups;

namespace ExchangeStuff.Service.Models.Screens
{
    public class ScreenPermissionViewModel : BaseEntity<Guid>
    {
        public int PermissionValue { get; set; }
        public PermisisonGroupViewModel PermissionGroup { get; set; }
        public ScreenViewModel Screen { get; set; }
    }
}
