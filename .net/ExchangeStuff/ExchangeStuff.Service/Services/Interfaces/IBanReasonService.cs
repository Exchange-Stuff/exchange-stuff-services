using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.BanReasons;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IBanReasonService
    {
        Task<BanReasonViewModel> GetBanReason(Guid id);
        Task<List<BanReasonViewModel>> GetBanReasons(string? content = null, BanReasonType? banReasonType = null!);
        Task<BanReasonViewModel> CreateBanReasons(BanReasonCreateModel banReasonCreateModel);
        Task<BanReasonViewModel> UpdateBanReasons(BanReasonUpdateModel banReasonUpdateModel);
    }
}
