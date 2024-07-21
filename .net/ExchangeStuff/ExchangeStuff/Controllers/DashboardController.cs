using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.Dashboard;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        //[ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("get-report-purchase")]
        public async Task<IActionResult> GetReportPurchase()
        {
            return Ok(new ResponseResult<ReportPurchaseTicket>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _dashboardService.GetReportPurchaseTicketThisWeek()
            });
        }
        //[ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("get-list-purchase-in-week")]
        public async Task<IActionResult> GetPurchasesInWeek()
        {
            return Ok(new ResponseResult<List<PurchaseTicketViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _dashboardService.GetListPurchaseThisWeek()
            });
        }
    }
}
