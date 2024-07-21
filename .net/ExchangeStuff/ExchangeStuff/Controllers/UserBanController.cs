using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.UserBanReports;
using ExchangeStuff.Service.Paginations;
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
        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserBanReport([FromRoute] Guid id)
        => Ok(new ResponseResult<UserBanReportViewModel>
        {
            Error = null!,
            IsSuccess = true,
            Value = await _userBanReportService.GetUserBanReport(id)
        });
        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("userBanReports/reasonIds")]
        public async Task<IActionResult> GetUserBanReportsReasonIds([FromQuery]List<Guid>? reasonIds = null, int? pageIndex = null, int? pageSize = null)
            => Ok(new ResponseResult<PaginationItem<UserBanReportViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _userBanReportService.GetUserBanReports(reasonIds: reasonIds, pageIndex: pageIndex, pageSize: pageSize)
            });
        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("userBanReports")]
        public async Task<IActionResult> GetUserBanReportsReasonIds(Guid? userId = null!, Guid? reasonId = null, string? reason = null, int? pageIndex = null, int? pageSize = null)
            => Ok(new ResponseResult<PaginationItem<UserBanReportViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _userBanReportService.GetUserBanReports(userId: userId, reasonId: reasonId, reason: reason, pageIndex: pageIndex, pageSize: pageSize)
            });
        [ESAuthorize(new string[] { ActionConstant.WRITE })]

        [HttpPost]
        public async Task<IActionResult> CreateUserBanReport([FromBody] UserBanReportCreateModel userBanReportCreateModel)
        {
            var rs = await _userBanReportService.CreateUserBanReport(userBanReportCreateModel);
            return rs != null ? StatusCode(StatusCodes.Status201Created, rs) : throw new Exception("Create user ban report fail");
        }


        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]

        [HttpPut("{id}")]
        public async Task<IActionResult> Approved([FromRoute] Guid id)
        {
            var rs = await _userBanReportService.ApproveUserBanReport(id);
            return rs ? StatusCode(StatusCodes.Status200OK, rs) : throw new Exception("Approve user ban report fail");
        }
    }
}
