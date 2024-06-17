using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Moderators
{
    public class ModeratorViewModel : BaseEntity<Guid>
    {
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
