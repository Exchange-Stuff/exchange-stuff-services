using ExchangeStuff.Core.Enums;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.TransactionHistory;
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

        [HttpGet("getAllTransactionHistory/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetAllTransactionHistory(int pageSize, int pageIndex, TransactionType status)
        {
            return Ok(new ResponseResult<List<TransactionHistoryViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _transactionHistoryService.GetAllTransactionHistory(pageSize, pageIndex, status)
            });
        }

        [HttpGet("getListTransactionHistoryByUserId/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetListTransactionHistoryByUserId(int pageSize, int pageIndex, TransactionType status)
        {
            return Ok(new ResponseResult<List<TransactionHistoryViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _transactionHistoryService.GetListTransactionHistoryByUserId(pageSize, pageIndex, status)
            });
        }

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
