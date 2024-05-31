using ExchangeStuff.Core.Entities;
using ExchangeStuff.Service.DTOs;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
        Task<string> GenerateToken(Admin admin);
        string GenerateRefreshToken();
        Task<bool> CheckRefreshTokenExpire(string token);
        Task SaveAccessToken(string token, Guid userId);
        Task<bool> DeleteAccessToken(string token);
        Task<bool> GetTokenValid(string token);
        Task<ClaimDTO> GetClaimDTOByAccessToken(string token);
        Task<string> RenewAccessToken(string refreshToken);
        ClaimDTO GetClaimDTOByAccessTokenSynchronous(string? token = null!);
    }
}
