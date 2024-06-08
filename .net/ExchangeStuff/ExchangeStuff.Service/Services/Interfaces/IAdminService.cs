using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Models.PermissionGroups;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.Users;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IAdminService
    {
        Task<bool> AddPermissionIntoPermissionGroup(UpdatePermissionActionValueModel updatePermissionActionValueModel);
        Task<bool> CreateAction(string name);
        Task<List<PermissionDTO>> GetPermissionsCache(List<Guid> permissionGroupIds);
        Task<List<PermissionDTO>> GetPermissionsCache();
        Task SavePermissionCache();
        Task<bool> ValidPermission(PermissionDTO permissionDTO, string resourceAccess);
        Task InvalidPermissionCache();
        Task<List<ActionDTO>> GetActionDTOsCache();
        Task SaveActionsCache();
        Task InvalidActionCache();
        Task<bool> CreatePermissionGroup(PermissionGroupCreateModel permissionGroupCreateModel);
        Task<bool> CreatePermissionGroupValue(PermissionGroupCreateValueModel permissionGroupCreateValueModel);
        Task<bool> UpdatePermissionActionValue(UpdatePermissionActionValueModel updatePermissionActionValueModel);
        Task<bool> UpdatePermissionGroupOfUser(UpdateUserPermisisonGroupModel updateUserPermisisonGroupModel);
    }
}
