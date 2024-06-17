using ExchangeStuff.Service.Models.UserBanReports;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IUserBanReportService
    {
        Task<UserBanReportViewModel> GetUserBanReport(Guid id);
        Task<List<UserBanReportViewModel>> GetUserBanReports(Guid? userId = null!, List<Guid>? reasonIds = null, Guid? reasonId = null, string? reason = null, int? pageIndex = null, int? pageSize = null);
        Task<UserBanReportViewModel> CreateUserBanReport(UserBanReportCreateModel userBanReportCreateModel);
        Task<bool> ApproveUserBanReport(Guid id);
    }
}
