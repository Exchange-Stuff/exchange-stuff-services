using ExchangeStuff.Core.Entities;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Models.Tokens;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface ITokenService
    {
        Task<ClaimDTO> GetClaimDTOByAccessToken(string? token = null!);
        Task<bool> CheckRefreshTokenExpire(string refreshToken, string accessToken);
        ClaimDTO GetClaimDTOByAccessTokenSynchronous(string? token = null!);
        Task<string> GenerateToken(Account account);
        Task<TokenViewModel> RenewAccessToken(string refreshToken);
        Task<string> GenerateToken(Admin admin);
        Task<ClaimDTO> GetClaimDTOByAccessTokenForReNew(string? token = null);
    }
}
