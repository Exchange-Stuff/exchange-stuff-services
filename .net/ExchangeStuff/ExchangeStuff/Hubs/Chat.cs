namespace ExchangeStuff.Hubs;

using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }
    public async Task JoinChatGeneral(Guid senderId)
    {
        var listGroup = await _chatService.JoinChatGeneral(senderId);
        if (DictonaryChat.connections.ContainsKey(senderId.ToString()))
        {
            DictonaryChat.connections[senderId.ToString()] = Context.ConnectionId;
        }
        else
        {
            DictonaryChat.connections.TryAdd(senderId.ToString(), Context.ConnectionId);
        }
        foreach (var item in listGroup)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, item.Id.ToString());
        }
    }
    public async Task SendMessage(string content, Guid receiverId, Guid senderId)
    {
        DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
        var formatCamelCase = new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        };
        try
        {
            var group = await _chatService.CreateMessage(receiverId, content, senderId);

            var listMsg = await _chatService.GetListMessage(group.Id);

            var listGroupSender = await _chatService.GetGroupsJoined(senderId);
            if (DictonaryChat.connections.TryGetValue(receiverId.ToString(), out string receiverConnection))
            {
                var listGroupReceiver = await _chatService.GetGroupsJoined(receiverId);
                await Clients.Client(receiverConnection).SendAsync("ReloadListJoin", JsonConvert.SerializeObject(listGroupReceiver, formatCamelCase));
            }
            await Clients.Client(Context.ConnectionId).SendAsync("ReloadListJoin", JsonConvert.SerializeObject(listGroupSender, formatCamelCase));
            await Clients.Group(group.Id.ToString()).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(listMsg, formatCamelCase));
        }
        catch (Exception ex)
        {
            var e = ex.Message;
        }
    }
}
