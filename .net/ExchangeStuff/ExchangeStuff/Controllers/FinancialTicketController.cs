using ExchangeStuff.Core.Enums;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.FinancialTickets;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialTicketController : ControllerBase
    {
        private readonly IFinancialTicketService _financialTicketService;

        public FinancialTicketController(IFinancialTicketService financialTicketService)
        {
            _financialTicketService = financialTicketService;
        }


        [HttpGet("/getAllFinancialTicket/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetFinancialTicket( int pageSize , int pageIndex, FinancialTicketStatus status )
        {
            return Ok(new ResponseResult<List<FinancialTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _financialTicketService.GetAllFinancialTicket(pageSize, pageIndex, status)

            });
        }
       

        [HttpGet("/getListFinancialTicketByUserId/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetListFinancialTicketByUserId( int pageSize,int pageIndex, FinancialTicketStatus status)
        {
            return Ok(new ResponseResult<List<FinancialTicketViewModel>>
            {
                Error = null,
                IsSuccess = true,
                Value = await _financialTicketService.GetFinancialTicketByUserId( pageSize, pageIndex, status)

            });

        }
        [HttpGet("/getFinancialTicketDetail/{financialTicketId}")]
        public async Task<IActionResult> GetFinancialTicketDetail(Guid financialTicketId)
        {
            return Ok(new ResponseResult<FinancialTicketViewModel>
            {
                Error = null,
                IsSuccess = true,
                Value = await _financialTicketService.GetFinancialTicketDetail(financialTicketId),
            });
        }
        [HttpPost("createFinancialTicket")]
        public async Task<IActionResult> CreateFinancialTicket([FromBody] CreateFinancialTicketModel financialTicket)
        {
            var rs = await _financialTicketService.CreateFinancialTicket(financialTicket);

            if (!rs) throw new Exception("Can't create financial ticket, CreateFinancialTicket");

            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

        [HttpPost("UpdateFinancialTicket")]
        public async Task<IActionResult> UpdatePurchaseTicket([FromBody] UpdateFinancialTicketModel financialTicket)
        {
            var rs = await _financialTicketService.UpdateFinancialTicket(financialTicket);

            if (!rs) throw new Exception("Can't update financial ticket, UpdateFiancialTicket");

            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

    }
}
