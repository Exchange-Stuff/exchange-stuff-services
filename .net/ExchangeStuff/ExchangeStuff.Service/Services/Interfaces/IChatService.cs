using ExchangeStuff.Service.Models.GroupChat;
using ExchangeStuff.Service.Models.MessageChat;
using System.Text.RegularExpressions;

namespace ExchangeStuff.Service.Services.Interfaces;

public interface IChatService
{
    Task<List<MessageChatViewModel>> GetListMessage(Guid groupId);
    Task<List<GroupChatViewModel>> JoinChatGeneral(Guid senderId);
    Task<List<GroupChatViewModel>> GetGroupsJoined(Guid senderId);
    Task<GroupChatViewModel> CreateMessage(Guid reviceId, string content, Guid senderId);
    Task<GroupChatViewModel> GetGroupBySenderReceiverId(Guid senderId, Guid receiverId);
    Task<GroupChatViewModel> CreateGroupAsync(Guid senderId, Guid receiverId);
    Task<GroupChatViewModel> CheckExisting(Guid senderId, Guid receiverId);
}
