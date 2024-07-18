using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Repositories.Base;

namespace ExchangeStuff.Repository.Repositories;

public class GroupChatRepository : Repository<GroupChat>, IGroupChatRepository
{
    public GroupChatRepository(ExchangeStuffContext context) : base(context)
    {
    }
}
