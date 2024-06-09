using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controller
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
            var listPurchaseTicket = await _purchaseTicketService.GetAllPurchaseTicket(pageSize, pageIndex, status);
            return Ok(listPurchaseTicket);
        }

        [HttpGet("/getListPurchaseTicketByUserId/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetListPurchaseTicketByUserId(int pageSize, int pageIndex, PurchaseTicketStatus status)
        {
            var listPurchaseTicket = await _purchaseTicketService.GetListPurchaseTicketByUserId(pageSize, pageIndex, status);
            return Ok(listPurchaseTicket);
        }

        [HttpGet("/getPurchaseTicketDetail/{purchaseTicketId}")]
        public async Task<IActionResult> GetPurchaseTicketDetail(Guid purchaseTicketId)
        {
            var listPurchaseTicket = await _purchaseTicketService.GetPurchaseTicketDetail(purchaseTicketId);
            return Ok(listPurchaseTicket);
        }
    }
}
