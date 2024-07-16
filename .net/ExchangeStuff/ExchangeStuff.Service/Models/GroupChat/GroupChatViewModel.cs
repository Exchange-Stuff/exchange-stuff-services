using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Service.Models.MessageChat;
using ExchangeStuff.Service.Models.Users;

namespace ExchangeStuff.Service.Models.GroupChat;

public class GroupChatViewModel : BaseEntity<Guid>
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }

    public UserViewModel Receiver { get; set; }
    public UserViewModel Sender { get; set; }

    public virtual ICollection<MessageChatViewModel>? MessageChats { get; set; } = new List<MessageChatViewModel>();
}
