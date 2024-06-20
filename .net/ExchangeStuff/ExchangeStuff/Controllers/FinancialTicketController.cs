﻿using ExchangeStuff.Core.Enums;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Comments;
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


        [HttpGet("/getAllFinancialTicket")]
        public async Task<IActionResult> GetFinancialTicket([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] FinancialTicketStatus status )
        {
            return Ok(new ResponseResult<List<FinancialTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _financialTicketService.GetAllFinancialTicket(pageSize, pageIndex, status)

            });
        }
       

        [HttpGet("/getListFinancialTicketByUserId")]
        public async Task<IActionResult> GetListFinancialTicketByUserId([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] FinancialTicketStatus status)
        {
            return Ok(new ResponseResult<List<FinancialTicketViewModel>>
            {
                Error = null,
                IsSuccess = true,
                Value = await _financialTicketService.GetFinancialTicketByUserId( pageSize, pageIndex, status)

            });

        }
        [HttpGet("/getFinancialTicketDetail/{id}")]
        public async Task<IActionResult> GetFinancialTicketDetail(Guid id)
        {
            return Ok(new ResponseResult<FinancialTicketViewModel>
            {
                Error = null,
                IsSuccess = true,
                Value = await _financialTicketService.GetFinancialTicketDetail(id),
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

        [HttpPut("UpdateFinancialTicket")]
        public async Task<IActionResult> UpdateFinancialTicket([FromBody] UpdateFinancialTicketModel financialTicket)
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
        [HttpGet("get-all-filter")]
        public async Task<IActionResult> GetAllFilter([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] FinancialTicketStatus? status, [FromQuery] int? sort)
        {
            return Ok(new ResponseResult<List<FinancialTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _financialTicketService.GetAllFilter(pageSize, pageIndex, from, to, status, sort)
            }); ;
        }
    }
}
