using ExchangeStuff.Core.Enums;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace ExchangeStuff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseTicketController : ControllerBase
    {
        private readonly IPurchaseTicketService _purchaseTicketService;

        public PurchaseTicketController(IPurchaseTicketService purchaseTicketService)
        {
            _purchaseTicketService = purchaseTicketService;
        }

        [HttpGet("/getAllPurchaseTicket/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus status)
        {
            return Ok(new ResponseResult<List<PurchaseTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _purchaseTicketService.GetAllPurchaseTicket(pageIndex, pageSize)
            });
        }

        [HttpGet("/getListPurchaseTicketByUserId/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetListPurchaseTicketByUserId(int pageSize, int pageIndex, PurchaseTicketStatus status)
        {
            return Ok(new ResponseResult<List<PurchaseTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _purchaseTicketService.GetListPurchaseTicketByUserId(pageSize, pageIndex, status)
            });
        }

        [HttpGet("/getPurchaseTicketDetail/{purchaseTicketId}")]
        public async Task<IActionResult> GetPurchaseTicketDetail(Guid purchaseTicketId)
        {
            return Ok(new ResponseResult<PurchaseTicketViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _purchaseTicketService.GetPurchaseTicketDetail(purchaseTicketId)
            });
        }

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

        [HttpPost("UpdatePurchaseTicket")]
        public async Task<IActionResult> UpdatePurchaseTicket([FromBody] UpdatePurchaseTicketModel purchaseTicket)
        {
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
