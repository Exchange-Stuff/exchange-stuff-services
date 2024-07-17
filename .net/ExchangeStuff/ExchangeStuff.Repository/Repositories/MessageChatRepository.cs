using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;

namespace ExchangeStuff.Repository.Repositories;

public class MessageChatRepository : Repository<MessageChat>, IMessageChatRepository
{
    public MessageChatRepository(ExchangeStuffContext context) : base(context)
    {
    }
}
