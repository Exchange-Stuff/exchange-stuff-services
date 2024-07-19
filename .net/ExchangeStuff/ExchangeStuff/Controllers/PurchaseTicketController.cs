using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseTicketController : ControllerBase
    {
        private readonly IPurchaseTicketService _purchaseTicketService;

        public PurchaseTicketController(IPurchaseTicketService purchaseTicketService)
        {
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
            UpdatePurchaseTicketModel up = new UpdatePurchaseTicketModel
            {
                Id = Guid.Parse("3EF7A762-B8D7-4D9A-CD41-08DCA6DD14B7"),
                Status = PurchaseTicketStatus.Processing
            };
            purchaseTicket = up;
            var rs = await _purchaseTicketService.UpdatePurchaseTicket(purchaseTicket);

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
