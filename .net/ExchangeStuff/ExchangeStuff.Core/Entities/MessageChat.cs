using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class MessageChat:Auditable<Guid>
    {
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public DateTime TimeSend { get; set; }
        public Guid GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; }
        public User Sender { get; set; }
    }
}
