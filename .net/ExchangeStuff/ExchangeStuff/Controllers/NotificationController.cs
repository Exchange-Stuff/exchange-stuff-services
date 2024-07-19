using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
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

        [ESAuthorize(new string[] { ActionConstant.READ })]
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

        [ESAuthorize(new string[] { ActionConstant.READ })]
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


        [ESAuthorize(new string[] { ActionConstant.WRITE })]

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateModel notificationCreateModel)
        {
            return StatusCode(StatusCodes.Status201Created, new ResponseResult<PaginationItem<NotificationViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _notificationService.CreateNotification(notificationCreateModel)
            });
        }
    }
}
