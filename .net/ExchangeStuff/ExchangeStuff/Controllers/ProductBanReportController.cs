using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.ProductBanReports;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBanReportController : ControllerBase
    {
        private readonly IProductBanReportService _productBanReportService;

        public ProductBanReportController(IProductBanReportService productBanReport)
        {
            _productBanReportService = productBanReport;
        }

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductBanReport([FromRoute] Guid id)
        => Ok(new ResponseResult<ProductBanReportViewModel>
        {
            Error = null!,
            IsSuccess = true,
            Value = await _productBanReportService.GetProductBanReport(id)
        });

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("productBanReports/reasonIds")]
        public async Task<IActionResult> GetProductBanReportsReasonIds(List<Guid>? reasonIds = null, int? pageIndex = null, int? pageSize = null)
            => Ok(new ResponseResult<PaginationItem<ProductBanReportViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _productBanReportService.GetProductBanReports(reasonIds: reasonIds, pageIndex: pageIndex, pageSize: pageSize)
            });

        [ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("productBanReports")]
        public async Task<IActionResult> GetProductBanReportsReasonIds(Guid? productId = null!, Guid? reasonId = null, string? reason = null, int? pageIndex = null, int? pageSize = null)
            => Ok(new ResponseResult<PaginationItem<ProductBanReportViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _productBanReportService.GetProductBanReports(userId: productId, reasonId: reasonId, reason: reason, pageIndex: pageIndex, pageSize: pageSize)
            });

        [ESAuthorize(new string[] { ActionConstant.WRITE })]
        [HttpPost]
        public async Task<IActionResult> CreateProductBanReport([FromBody] ProductBanReportCreateModel ProductBanReportCreateModel)
        {
            var rs = await _productBanReportService.CreateProductBanReport(ProductBanReportCreateModel);
            return rs != null ? StatusCode(StatusCodes.Status201Created, rs) : throw new Exception("Create user ban report fail");
        }

        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]
        [HttpPut("{id}")]
        public async Task<IActionResult> Approved([FromRoute] Guid id)
        {
            var rs = await _productBanReportService.ApproveProductBanReport(id);
            return rs ? StatusCode(StatusCodes.Status200OK, rs) : throw new Exception("Approve user ban report fail");
        }
    }
}
