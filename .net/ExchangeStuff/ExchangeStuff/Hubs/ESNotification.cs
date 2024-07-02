using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ExchangeStuff.Hubs
{
    /// <summary>
    /// pipeline
    /// </summary>
    public class ESNotification : Hub
    {
        private readonly ICacheService _cacheService;

        public ESNotification(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public override async Task OnConnectedAsync()
        {
            await _cacheService.AddConnection(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public async Task SendNotification(string accountId, string msg)
        {
            var connectionIds = await _cacheService.GetConnectionId((accountId+"").ToLower());
            if (connectionIds.Count>0)
            {
                foreach (var item in connectionIds)
                {
                    await Clients.Client(item).SendAsync("ReceiveNotification", msg);
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _cacheService.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
