using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.TransactionHistory;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public TransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getAllTransactionHistory/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetAllTransactionHistory(int pageSize, int pageIndex, TransactionType status)
        {
            return Ok(new ResponseResult<PaginationItem<TransactionHistoryViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _transactionHistoryService.GetAllTransactionHistory(pageSize, pageIndex, status)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getListTransactionHistoryByUserId/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> GetListTransactionHistoryByUserId(int pageSize, int pageIndex)
        {
            return Ok(new ResponseResult<PaginationItem<TransactionHistoryViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _transactionHistoryService.GetListTransactionHistoryByUserId(pageSize, pageIndex)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("getTransactionHistoryDetail/{transactionHistoryId}")]
        public async Task<IActionResult> GetTransactionHistoryDetail(Guid transactionHistoryId)
        {
            return Ok(new ResponseResult<TransactionHistoryViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _transactionHistoryService.GetTransactionHistoryDetail(transactionHistoryId)
            });
        }
        [ESAuthorize(new string[] { ActionConstant.WRITE })]
        [HttpPost("createTransactionHistory")]
        public async Task<IActionResult> CreateTransactionHistory([FromBody] CreateTransactionHistoryModel transactionHistory)
        {
            var rs = await _transactionHistoryService.CreateTransactionHistory(transactionHistory);

            if (!rs) throw new Exception("Can't create transaction history, CreateTransactionHistory");

            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

    }
}
