using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Actions;
using ExchangeStuff.Service.Models.Admins;
using ExchangeStuff.Service.Models.PermissionGroups;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.Resources;
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
        Task<bool> UpdatePermissionGroupOfAccount(UpdateUserPermisisonGroupModel updateUserPermisisonGroupModel);
        Task<List<ActionViewModel>> GetActions(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<List<PermissionViewModel>> GetPermissions(int? pageIndex = null!, int? pageSize = null!);
        Task<List<ResourceViewModel>> GetResources(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<List<PermisisonGroupViewModel>> GetPermisisonGroups(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<string> LoginAdmin(string username, string password);
        Task<AdminViewModel> CreateAdmin(string username, string password, string name);
        Task<bool> CreateResource(string name);
        Task<bool> CreateAccount(AccountCreateModel accountCreateModel);
        Task<bool> DeletePermissionGroup(Guid id);
    }
}
