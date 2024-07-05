using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class Notification : Auditable<Guid>
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }
}
