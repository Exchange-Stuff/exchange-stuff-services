using ExchangeStuff.Service.DTOs;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface ICacheService
    {
        Task<bool> GetTokenValid(string token);
        Task SaveAccessToken(string token, Guid userId);
        Task<bool> DeleteAccessToken(string token);
        Task SavePermissionGroup(Guid id);
        Task InvalidPermissionGroup(Guid id);
        Task<List<PermissionGroupDTO>> GetPermissionGroupByAccountId(Guid id);
    }
}
