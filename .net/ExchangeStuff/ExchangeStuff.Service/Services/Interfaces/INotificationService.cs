using ExchangeStuff.Service.Models.Notifications;
using ExchangeStuff.Service.Paginations;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface INotificationService
    {
        Task<PaginationItem<NotificationViewModel>> CreateNotification(NotificationCreateModel notification);
        Task<PaginationItem<NotificationViewModel>> ReadAllByAccountId(int sizeRecent);
        Task<PaginationItem<NotificationViewModel>> GetNotifications(int? pageIndex = null!, int? pageSize = null!);
    }
}
