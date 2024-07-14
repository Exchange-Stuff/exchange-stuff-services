using ExchangeStuff.Core.Entities;
using ExchangeStuff.Service.DTOs;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface ICacheService
    {
        Task<bool> GetTokenValid(string token);
        Task<Token> SaveAccessToken(string token, Guid accountId);
        Task<bool> DeleteAccessToken(string token);
        Task SavePermissionGroup(Guid id);
        Task InvalidPermissionGroup(Guid id);
        Task<List<PermissionGroupDTO>> GetPermissionGroupByAccountId(Guid id);
        Task SavePermissionGroupAdmin(Guid id);
        Task InvalidAllSession(Guid accId);
        Task AddConnection(string connectionId);
        Task RemoveConnection(string connectionId);
        Task<List<string>> GetConnectionId(string accountId);
    }
}
