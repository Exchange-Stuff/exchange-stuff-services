using ExchangeStuff.Core.Entities;
using ExchangeStuff.Service.DTOs;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface ITokenService
    {
        Task<ClaimDTO> GetClaimDTOByAccessToken(string? token = null!);
        Task<bool> CheckRefreshTokenExpire(string token);
        ClaimDTO GetClaimDTOByAccessTokenSynchronous(string? token = null!);
        Task<string> GenerateToken(Account account);
        Task<string> RenewAccessToken(string refreshToken);
        Task<string> GenerateToken(Admin admin);
    }
}
