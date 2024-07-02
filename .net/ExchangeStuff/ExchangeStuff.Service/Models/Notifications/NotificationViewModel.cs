using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Service.Models.Notifications
{
    public class NotificationViewModel:Auditable<Guid>
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
