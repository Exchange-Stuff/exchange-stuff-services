using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.UserBanReports;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBanController : ControllerBase
    {
        private readonly IUserBanReportService _userBanReportService;

        public UserBanController(IUserBanReportService userBanReport)
        {
            _userBanReportService = userBanReport;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserBanReport([FromRoute] Guid id)
        => Ok(new ResponseResult<UserBanReportViewModel>
        {
            Error = null!,
            IsSuccess = true,
            Value = await _userBanReportService.GetUserBanReport(id)
        });

        [HttpGet("userBanReports/reasonIds")]
        public async Task<IActionResult> GetUserBanReportsReasonIds(List<Guid>? reasonIds = null, int? pageIndex = null, int? pageSize = null)
            => Ok(new ResponseResult<List<UserBanReportViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _userBanReportService.GetUserBanReports(reasonIds: reasonIds, pageIndex: pageIndex, pageSize: pageSize)
            });

        [HttpGet("userBanReports")]
        public async Task<IActionResult> GetUserBanReportsReasonIds(Guid? userId = null!, Guid? reasonId = null, string? reason = null, int? pageIndex = null, int? pageSize = null)
            => Ok(new ResponseResult<List<UserBanReportViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _userBanReportService.GetUserBanReports(userId: userId, reasonId: reasonId, reason: reason, pageIndex: pageIndex, pageSize: pageSize)
            });

        [HttpPost]
        public async Task<IActionResult> CreateUserBanReport([FromBody] UserBanReportCreateModel userBanReportCreateModel)
        {
            var rs = await _userBanReportService.CreateUserBanReport(userBanReportCreateModel);
            return rs != null ? StatusCode(StatusCodes.Status201Created, rs) : throw new Exception("Create user ban report fail");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Approved([FromRoute] Guid id)
        {
            var rs = await _userBanReportService.ApproveUserBanReport(id);
            return rs ? StatusCode(StatusCodes.Status200OK, rs) : throw new Exception("Approve user ban report fail");
        }
    }
}
