using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Notifications;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpPut("readAll")]
        public async Task<IActionResult> ReadAllNotification([FromBody] int sizeRecent)
        {
            return Ok(new ResponseResult<PaginationItem<NotificationViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _notificationService.ReadAllByAccountId(sizeRecent)
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications(int? pageIndex = null!, int? pageSize = null!)
        {
            return Ok(new ResponseResult<PaginationItem<NotificationViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _notificationService.GetNotifications(pageIndex: pageIndex, pageSize: pageSize)
            });
        }
    }
}
