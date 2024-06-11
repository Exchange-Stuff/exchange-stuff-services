using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Admins
{
    public class AdminViewModel : BaseEntity<Guid>
    {
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
