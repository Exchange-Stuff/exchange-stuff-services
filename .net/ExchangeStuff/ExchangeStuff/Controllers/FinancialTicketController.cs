﻿using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.Comments;
using ExchangeStuff.Service.Models.FinancialTickets;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialTicketController : ControllerBase
    {
        private readonly INotificationService _notiService;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IFinancialTicketService _financialTicketService;

        public FinancialTicketController(IFinancialTicketService financialTicketService, INotificationService notificationService, IIdentityUser<Guid> identityUser)
        {
            _notiService = notificationService;
            _identityUser = identityUser;
            _financialTicketService = financialTicketService;
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("getAllFinancialTicket")]
        public async Task<IActionResult> GetFinancialTicket([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] FinancialTicketStatus? status )

        {
            return Ok(new ResponseResult<PaginationItem<FinancialTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _financialTicketService.GetAllFinancialTicket(pageSize, pageIndex, status)

            });
        }

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getListFinancialTicketByUserId/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetListFinancialTicketByUserId(int pageSize, int pageIndex, FinancialTicketStatus? status)
        {
            return Ok(new ResponseResult<PaginationItem<FinancialTicketViewModel>>
            {
                Error = null,
                IsSuccess = true,
                Value = await _financialTicketService.GetFinancialTicketByUserId(pageSize, pageIndex, status)

            });

        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getFinancialTicketDetail/{id}")]
        public async Task<IActionResult> GetFinancialTicketDetail(Guid id)
        {
            return Ok(new ResponseResult<FinancialTicketViewModel>
            {
                Error = null,
                IsSuccess = true,
                Value = await _financialTicketService.GetFinancialTicketDetail(id),
            });
        }
        [ESAuthorize(new string[] { ActionConstant.WRITE })]
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
        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]
        [HttpPut("UpdateFinancialTicket")]
        public async Task<IActionResult> UpdateFinancialTicket([FromBody] UpdateFinancialTicketModel financialTicket)
        {
            var rs = await _financialTicketService.UpdateFinancialTicket(financialTicket);

            if (!rs) throw new Exception("Can't update financial ticket, UpdateFiancialTicket");
            FinancialTicketViewModel final = await _financialTicketService.GetFinancialTicketDetail(financialTicket.Id);
            if (final != null)
            {
                await _notiService.CreateNotification(new Service.Models.Notifications.NotificationCreateModel
                {
                    AccountId = final.CreatedBy,
                    Message = $"Số tiền {final.Amount} xu của bạn đã được duyệt"
                });
            }
            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("get-all-filter")]
        public async Task<IActionResult> GetAllFilter([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] FinancialTicketStatus? status, [FromQuery] int? sort)
        {
            return Ok(new ResponseResult<PaginationItem<FinancialTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _financialTicketService.GetAllFilter(pageSize, pageIndex, from, to, status, sort)
            }); ;
        }
    }
}
