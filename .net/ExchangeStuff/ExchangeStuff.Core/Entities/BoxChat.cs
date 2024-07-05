using ExchangeStuff.Core.Common;

namespace ExchangeStuff.Core.Entities
{
    public class BoxChat:Auditable<Guid>
    {
        public string Name { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<MessageChat>? MesageChats { get; set; }
    }
}
