using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Impls;

using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseTicketController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly INotificationService _notiService;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IPurchaseTicketService _purchaseTicketService;

        public PurchaseTicketController(IPurchaseTicketService purchaseTicketService, IIdentityUser<Guid> identityUser, INotificationService notificationService, IProductService productService)
        {
            _productService = productService;
            _notiService = notificationService;
            _identityUser = identityUser;
            _purchaseTicketService = purchaseTicketService;
        }

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getAllPurchaseTicket/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus status)
        {
            return Ok(new ResponseResult<PaginationItem<PurchaseTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _purchaseTicketService.GetAllPurchaseTicket(pageSize, pageIndex, status)
            });

        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getListPurchaseTicketByUserId/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetListPurchaseTicketByUserId(int pageSize, int pageIndex, PurchaseTicketStatus status)
        {
            return Ok(new ResponseResult<PaginationItem<PurchaseTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _purchaseTicketService.GetListPurchaseTicketByUserId(pageSize, pageIndex, status)
            });
        }

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getListPurchaseTicketForSeller/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetListPurchaseTicketForSeller(int pageSize, int pageIndex, PurchaseTicketStatus status)
        {
            return Ok(new ResponseResult<PaginationItem<PurchaseTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _purchaseTicketService.GetListPurchaseTicketForSeller(pageSize, pageIndex, status)
            });
        }

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getPurchaseTicketDetail/{purchaseTicketId}")]
        public async Task<IActionResult> GetPurchaseTicketDetail(Guid purchaseTicketId)
        {
            return Ok(new ResponseResult<PurchaseTicketViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _purchaseTicketService.GetPurchaseTicketDetail(purchaseTicketId)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.WRITE })]
        [HttpPost("createPurchaseTicket")]
        public async Task<IActionResult> CreatePurchaseTicket([FromBody] CreatePurchaseTicketModel purchaseTicket)
        {
            var rs = await _purchaseTicketService.CreatePurchaseTicket(purchaseTicket);

            if (!rs) throw new Exception("Can't create purchase ticket, CreatePurchaseTicket");
            var product = await _productService.GetDetail(purchaseTicket.ProductId);
            if (product != null)
            {
                await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
                {
                    AccountId = product.CreatedBy,
                    Message = "Đã có người dùng đặt hàng sản phẩm của bạn"
                });
                await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
                {
                    AccountId = _identityUser.AccountId,
                    Message = $"Bạn đã bị trừ {purchaseTicket.Amount} xu vào ví"
                });
            }
            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }
        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]
        [HttpPut("UpdatePurchaseTicket")]
        public async Task<IActionResult> UpdatePurchaseTicket([FromBody] UpdatePurchaseTicketModel purchaseTicket)
        {
            var rs = await _purchaseTicketService.UpdatePurchaseTicket(purchaseTicket);
            var purchaseTicketget = await _purchaseTicketService.GetPurchaseTicketDetail(purchaseTicket.Id);
            if (purchaseTicketget != null)
            {
                if (purchaseTicket.Status == PurchaseTicketStatus.Cancelled)
                {
                    await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
                    {
                        AccountId = purchaseTicketget.UserId,
                        Message = $"bạn được nhận lại {purchaseTicketget.Amount} xu vào ví"
                    });
                    await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
                    {
                        AccountId = purchaseTicketget.Product.CreatedBy,
                        Message = $"Đơn hàng #{purchaseTicketget.Id} của bạn đã bị huỷ"
                    });
                }
                else if (purchaseTicket.Status == PurchaseTicketStatus.Confirmed)
                {
                    await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
                    {
                        AccountId = purchaseTicketget.Product.CreatedBy,
                        Message = $"Đơn hàng #{purchaseTicketget.Id} của bạn được xác nhận"
                    });
                }
            }

            if (!rs) throw new Exception("Can't update purchase ticket, UpdatePurchaseTicket");

            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

    }
}
