using ExchangeStuff.Service.Models.ProductBanReports;
using ExchangeStuff.Service.Models.UserBanReports;
using ExchangeStuff.Service.Paginations;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IProductBanReportService
    {
        Task<ProductBanReportViewModel> GetProductBanReport(Guid id);
        Task<PaginationItem<ProductBanReportViewModel>> GetProductBanReports(Guid? userId = null!, List<Guid>? reasonIds = null, Guid? reasonId = null, string? reason = null, int? pageIndex = null, int? pageSize = null);
        Task<ProductBanReportViewModel> CreateProductBanReport(ProductBanReportCreateModel productBanReportCreateModel);
        Task<bool> ApproveProductBanReport(Guid id);
    }
}
