using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class ParticipantChat:Auditable<Guid>
    {
        public Guid UserId { get; set; }
        public Guid BoxChatId { get; set; }
        public User User { get; set; }
        public BoxChat BoxChat { get; set; }
    }
}
