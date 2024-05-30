using ExchangeStuff.Application.DTOs;
using ExchangeStuff.Application.Services;
using ExchangeStuff.Core.Entities;

namespace ExchangeStuff.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        public Task<bool> CheckRefreshTokenExpire(string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAccessToken(string token)
        {
            throw new NotImplementedException();
        }

        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateToken(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateToken(Admin admin)
        {
            throw new NotImplementedException();
        }

        public Task<ClaimDTO> GetClaimDTOByAccessToken(string token)
        {
            throw new NotImplementedException();
        }

        public ClaimDTO GetClaimDTOByAccessTokenSynchronous(string? token = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTokenValid(string token)
        {
            throw new NotImplementedException();
        }

        public Task<string> RenewAccessToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task SaveAccessToken(string token, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
