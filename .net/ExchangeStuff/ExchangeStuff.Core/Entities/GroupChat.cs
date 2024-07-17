using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class GroupChat : Auditable<Guid>
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }

        public User Receiver { get; set; }
        public User Sender { get; set; }

        public ICollection<MessageChat>? MessageChats { get; set; }
    }
}
