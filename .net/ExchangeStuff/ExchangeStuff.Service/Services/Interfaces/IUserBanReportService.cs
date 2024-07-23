using ExchangeStuff.Service.Models.UserBanReports;
using ExchangeStuff.Service.Paginations;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IUserBanReportService
    {
        Task<UserBanReportViewModel> GetUserBanReport(Guid id);
        Task<PaginationItem<UserBanReportViewModel>> GetUserBanReports(Guid? userId = null!, List<Guid>? reasonIds = null, Guid? reasonId = null, string? reason = null, int? pageIndex = null, int? pageSize = null);
        Task<UserBanReportViewModel> CreateUserBanReport(UserBanReportCreateModel userBanReportCreateModel);
        Task<bool> ApproveUserBanReport(Guid id);
        Task<UserBanReportViewModel> GetUserBan(Guid id);
    }
}
