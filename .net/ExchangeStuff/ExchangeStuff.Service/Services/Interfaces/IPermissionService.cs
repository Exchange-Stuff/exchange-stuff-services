using ExchangeStuff.Service.DTOs;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionDTO>> GetPermissionsCache(List<Guid> permissionGroupIds);
        Task<List<PermissionDTO>> GetPermissionsCache();
        Task SavePermissionCache();
        Task<bool> ValidPermission(PermissionDTO permissionDTO, string resourceAccess);
        Task InvalidPermissionCache();
    }
}
