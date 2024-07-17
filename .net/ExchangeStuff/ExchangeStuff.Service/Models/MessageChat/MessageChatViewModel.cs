using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.MessageChat;

public class MessageChatViewModel : BaseEntity<Guid>
{
    public Guid SenderId { get; set; }
    public string Content { get; set; }
    public DateTime TimeSend { get; set; }
    public Guid GroupChatId { get; set; }
}
