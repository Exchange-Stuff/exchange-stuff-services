using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.GroupChat;
using ExchangeStuff.Service.Models.MessageChat;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.RegularExpressions;

namespace ExchangeStuff.Service.Services.Impls;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGroupChatRepository _groupChatRepostiory;
    private readonly IMessageChatRepository _messageChatRepository;
    private readonly IIdentityUser<Guid> _identityUser;

    public ChatService(IUnitOfWork unitOfWork , IIdentityUser<Guid> identityUser)
    {
        _identityUser = identityUser;
        _unitOfWork = unitOfWork;
        _groupChatRepostiory = _unitOfWork.GroupChatRepository;
        _messageChatRepository = _unitOfWork.MessageChatRepository;
    }

    public async Task<GroupChatViewModel> CreateMessage(Guid reviceId, string content, Guid senderId)
    {
        var group = await GetGroupBySenderReceiverId(senderId, reviceId);
        if (group != null)
        {
            var newMessage = new MessageChat()
            {
                Id = Guid.NewGuid(),
                Content = content,
                TimeSend = DateTime.Now,
                SenderId = senderId,
                GroupChatId = group.Id,
            };
            await _messageChatRepository.AddAsync(newMessage);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return group;
            }
            return null;
        }
        else
        {
            var groupNew = await CreateGroupAsync(senderId, reviceId);
            var newMessage = new MessageChat()
            {
                Id = Guid.NewGuid(),
                Content = content,
                TimeSend = DateTime.Now,
                SenderId = senderId,
                GroupChatId = groupNew.Id,
            };
            await _messageChatRepository.AddAsync(newMessage);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return groupNew;
            }
            return null;
        }
    }

    public async Task<GroupChatViewModel> CreateGroupAsync(Guid senderId, Guid receiverId)
    {
        GroupChat newGroup = new GroupChat()
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = receiverId,
        };
        await _groupChatRepostiory.AddAsync(newGroup);
        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return AutoMapperConfig.Mapper.Map<GroupChatViewModel>(newGroup);
        }
        return null;
    }

    public async Task<GroupChatViewModel> GetGroupBySenderReceiverId(Guid senderId, Guid receiverId)
    {
        var group = await _groupChatRepostiory.GetOneAsync(predicate: g => (g.SenderId == senderId && g.ReceiverId == receiverId) || (g.SenderId == receiverId && g.ReceiverId == senderId));
        return AutoMapperConfig.Mapper.Map<GroupChatViewModel>(group);
    }

    public async Task<List<GroupChatViewModel>> GetGroupsJoined(Guid senderId)
    {
        var list = await _groupChatRepostiory.GetManyAsync(
            predicate: g => g.SenderId == senderId || g.ReceiverId == senderId,
            include: "MessageChats,Receiver,Sender");
        foreach (var group in list)
        {
            var msg = group.MessageChats.OrderByDescending(x => x.TimeSend).FirstOrDefault();
            if (msg != null)
            {
                group.MessageChats.Clear();
                group.MessageChats.Add(msg);
            }
        }
        var result = list.OrderByDescending(x => x.MessageChats?.FirstOrDefault()?.TimeSend).ToList();
        return AutoMapperConfig.Mapper.Map<List<GroupChatViewModel>>(result);
    }

    public async Task<List<MessageChatViewModel>> GetListMessage(Guid groupId)
    {
        var listMessage = await _messageChatRepository.GetManyAsync(predicate: m => m.GroupChatId == groupId, orderBy: m => m.OrderByDescending(m => m.TimeSend), pageIndex: 1, pageSize: 20);
        return AutoMapperConfig.Mapper.Map<List<MessageChatViewModel>>(listMessage.OrderBy(m => m.TimeSend));
    }

    public async Task<List<GroupChatViewModel>> JoinChatGeneral(Guid senderId)
    {
        var listGroup = await _groupChatRepostiory.GetManyAsync(predicate: g => g.SenderId == senderId || g.ReceiverId == senderId);
        return AutoMapperConfig.Mapper.Map<List<GroupChatViewModel>>(listGroup);
    }

    public async Task<GroupChatViewModel> CheckExisting(Guid senderId, Guid receiverId)
    {
        var group = await _groupChatRepostiory.GetOneAsync(
            predicate: g => (g.SenderId == senderId && g.ReceiverId == receiverId) || (g.ReceiverId == senderId && g.SenderId == receiverId),
            include: "MessageChats,Receiver,Sender");
        if(group != null)
        {
            return AutoMapperConfig.Mapper.Map<GroupChatViewModel>(group);
        }
        var groupNew = await CreateGroupAsync(senderId, receiverId);
        var result = await _groupChatRepostiory.GetOneAsync(
           predicate: g => (g.SenderId == senderId && g.ReceiverId == receiverId) || (g.ReceiverId == senderId && g.SenderId == receiverId),
           include: "MessageChats,Receiver,Sender");
        return AutoMapperConfig.Mapper.Map<GroupChatViewModel>(result);
    }
}
