using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controller
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

        [HttpGet("/getAllTransactionHistory/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetAllTransactionHistory(int pageSize, int pageIndex, TransactionType status)
        {
            var listTransactionHistory = await _transactionHistoryService.GetAllTransactionHistory(pageSize, pageIndex, status);
            return Ok(listTransactionHistory);
        }

        [HttpGet("/getListTransactionHistoryByUserId/{pageSize}/{pageIndex}/{status}")]
        public async Task<IActionResult> GetListTransactionHistoryByUserId(int pageSize, int pageIndex, TransactionType status)
        {
            var listTransactionHistory = await _transactionHistoryService.GetListTransactionHistoryByUserId(pageSize, pageIndex, status);
            return Ok(listTransactionHistory);
        }

        [HttpGet("/getTransactionHistoryDetail/{transactionHistoryId}")]
        public async Task<IActionResult> GetTransactionHistoryDetail(Guid transactionHistoryId)
        {
            var listPurchaseTicket = await _transactionHistoryService.GetTransactionHistoryDetail(transactionHistoryId);
            return Ok(listPurchaseTicket);
        }
    }
}
