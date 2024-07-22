using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Hubs;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Notifications;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace ExchangeStuff.Service.Services.Impls
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ESNotification> _hubContext;
        private readonly ESNotification _hub;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IUnitOfWork _uow;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IUnitOfWork unitOfWork, IIdentityUser<Guid> identityUser, IHubContext<ESNotification> hubContext, ICacheService cacheService)
        {
            _hubContext = hubContext;
            _hub = new ESNotification(cacheService);
            _identityUser = identityUser;
            _uow = unitOfWork;
            _notificationRepository = _uow.NotificationRepository;
        }

        public async Task<PaginationItem<NotificationViewModel>> CreateNotification(NotificationCreateModel notification)
        {
            Notification notificationNew = new Notification
            {
                Message = notification.Message,
                AccountId = notification.AccountId,
                IsRead = false
            };
            await _notificationRepository.AddAsync(notificationNew);
            await _uow.SaveChangeAsync();
            await _hub.SendNotification(notificationNew.AccountId + "", notificationNew.Message, _hubContext);
            return await GetNotifications(pageIndex: 0, pageSize: 50);
        }

        public async Task<PaginationItem<NotificationViewModel>> GetNotifications(int? pageIndex = null, int? pageSize = null)
        {
            if (_identityUser.AccountId == Guid.Empty) throw new UnauthorizedAccessException("Login please");
            var notiNew = AutoMapperConfig.Mapper.Map<List<NotificationViewModel>>(await _notificationRepository.GetManyAsync(predicate:x=>x.AccountId==_identityUser.AccountId, pageIndex: pageIndex, pageSize: pageSize));
            return PaginationItem<NotificationViewModel>.ToPagedList(notiNew, pageSize: pageSize, pageIndex: pageIndex);
        }

        public async Task<PaginationItem<NotificationViewModel>> ReadAllByAccountId(int sizeRecent)
        {
            if (_identityUser.AccountId != Guid.Empty)
            {
                var notis = await _notificationRepository.GetManyAsync(pageIndex: 0, pageSize: sizeRecent, forUpdate: true);
                foreach (var item in notis)
                {
                    item.IsRead = true;
                }
                _notificationRepository.UpdateRange(notis);
                await _uow.SaveChangeAsync();
            }
            var notiNew = AutoMapperConfig.Mapper.Map<List<NotificationViewModel>>(await _notificationRepository.GetManyAsync(pageIndex: 0, pageSize: 50));
            return await GetNotifications(pageIndex: 0, pageSize: 50);
        }
    }
}
