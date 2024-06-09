using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Actions;
using ExchangeStuff.Service.Models.PermissionGroups;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.Resources;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ExchangeStuff.Service.Services.Impls
{
    public class AdminService : TokenService, IAdminService
    {
        private readonly IUnitOfWork _uow;
        private readonly IActionRepository _actionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionGroupRepository _permissionGroupRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly RedisDTO _redisDTO = new();
        private readonly RedisConstantDTO _redisConstantDTO = new RedisConstantDTO();

        public AdminService(IUnitOfWork unitOfWork, IConfiguration configuration, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, configuration, httpContextAccessor, distributedCache, connectionMultiplexer)
        {
            _uow = unitOfWork;
            _actionRepository = _uow.ActionRepository;
            _permissionRepository = _uow.PermissionRepository;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _connectionMutiple = connectionMultiplexer;
            _permissionRepository = _uow.PermissionRepository;
            _permissionGroupRepository = _uow.PermissionGroupRepository;
            _accountRepository = _uow.AccountRepository;
            _resourceRepository = _uow.ResourceRepository;
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisConstantDTO);
        }

        public async Task<bool> AddPermissionIntoPermissionGroup(UpdatePermissionActionValueModel updatePermissionActionValueModel)
        {
            if (updatePermissionActionValueModel.ResourceValueRecords.Any() is false) throw new ArgumentNullException("Action is required");
            var actionSystem = await _actionRepository.GetManyAsync();
            var permissions = await _permissionRepository.GetManyAsync(x => x.PermissionGroupId == updatePermissionActionValueModel.PermissionGroupId);

            _permissionRepository.RemoveRange(permissions);
            List<Permission> permissionList = new List<Permission>();
            List<Permission> permissionListUpdate = new List<Permission>();

            foreach (var item in updatePermissionActionValueModel.ResourceValueRecords)
            {
                Permission newPermission = new Permission
                {
                    Id = Guid.NewGuid(),
                    PermissionValue = item.PermissionValue,
                    PermissionGroupId = updatePermissionActionValueModel.PermissionGroupId,
                    ResourceId = item.ResourceId
                };
                permissionList.Add(newPermission);
            }
            await _permissionRepository.AddRangeAsync(permissionList);
            var rs = await _uow.SaveChangeAsync();
            await InvalidPermissionCache();
            return rs > 0;
        }

        public async Task<bool> CreateAction(string name)
        {
            var actions = await _actionRepository.GetManyAsync();
            if (actions.Any() is false) throw new Exception("No action in system");
            var actionCheck = actions.Where(x => x.Name.ToLower() == name.ToLower()).ToList();
            if (actionCheck.Any()) throw new Exception("Action already exist in system");

            actions = actions.OrderBy(x => x.Index).ToList();
            ExchangeStuff.Core.Entities.Action action = new Core.Entities.Action
            {
                Id = Guid.NewGuid(),
                Index = actions.Last().Index + 1,
                Value = (int)Math.Pow(2, (actions.Last().Index + 1)),
                Name = name,
            };
            await _actionRepository.AddAsync(action);
            var rs = await _uow.SaveChangeAsync();
            await InvalidActionCache();
            return rs > 0;
        }

        public async Task<List<PermissionDTO>> GetPermissionsCache(List<Guid> permissionGroupIds)
        {
            if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var permissionString = await _distributedCache.GetStringAsync(_redisConstantDTO.PermissionResource);
                if (permissionString != null)
                {
                    var permissions = JsonConvert.DeserializeObject<List<PermissionDTO>>(permissionString);
                    if (permissions != null!)
                    {
                        return permissions.Where(x => permissionGroupIds.Contains(x.PermissionGroupId)).ToList();
                    }
                }
            }
            return AutoMapperConfig.Mapper.Map<List<PermissionDTO>>(await _permissionRepository.GetManyAsync(x => permissionGroupIds.Contains(x.PermissionGroupId), "PermissionGroup,Resource"));
        }

        public async Task<List<PermissionDTO>> GetPermissionsCache()
        {
            if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var permissionString = await _distributedCache.GetStringAsync(_redisConstantDTO.PermissionResource);
                if (permissionString != null)
                {
                    return JsonConvert.DeserializeObject<List<PermissionDTO>>(permissionString)!;
                }
            }
            return AutoMapperConfig.Mapper.Map<List<PermissionDTO>>(await _permissionRepository.GetManyAsync(include: "PermissionGroup,Resource"));
        }

        public async Task InvalidPermissionCache()
        {
            await _distributedCache.RemoveAsync(_redisConstantDTO.PermissionResource);
        }

        public async Task SavePermissionCache()
        {
            var permission = await GetPermissionsCache();
            var json = JsonConvert.SerializeObject(AutoMapperConfig.Mapper.Map<List<PermissionDTO>>(permission));
            await _distributedCache.SetStringAsync(_redisConstantDTO.PermissionResource, json);
        }

        public Task<bool> ValidPermission(PermissionDTO permissionDTO, string resourceAccess)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ActionDTO>> GetActionDTOsCache()
        {
            if (_redisDTO == null!) throw new ArgumentNullException("Can't bind redis server");
            if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var actionString = await _distributedCache.GetStringAsync(_redisConstantDTO.ActionsResource);

                if (actionString != null)
                {
                    return JsonConvert.DeserializeObject<List<ActionDTO>>(actionString)!;
                }
            }
            var actions = await _actionRepository.GetManyAsync();
            if (actions == null! || actions.Count == 0) throw new ArgumentNullException("Not found user authorize");
            return AutoMapperConfig.Mapper.Map<List<ActionDTO>>(actions);
        }

        public async Task InvalidActionCache()
        {
            await _distributedCache.RemoveAsync(_redisConstantDTO.ActionsResource);
        }

        public async Task SaveActionsCache()
        {
            var actions = await _actionRepository.GetManyAsync();
            if (actions == null! || actions.Count == 0) throw new ArgumentNullException("Not found user authorize");
            await _distributedCache.SetStringAsync(_redisConstantDTO.ActionsResource, JsonConvert.SerializeObject(AutoMapperConfig.Mapper.Map<List<ActionDTO>>(actions)));
        }

        public async Task<bool> CreatePermissionGroup(PermissionGroupCreateModel permissionGroupCreateModel)
        {
            var roleCheck = await _permissionGroupRepository.GetManyAsync(x => x.Name.ToLower() == permissionGroupCreateModel.Name.ToLower());
            if (roleCheck.Any()) throw new Exception("Permission group name already exist");
            var newPer = new PermissionGroup
            {
                Id = Guid.NewGuid(),
                Name = permissionGroupCreateModel.Name
            };
            var actionSystem = await _actionRepository.GetManyAsync();
            List<Permission> permissions = new List<Permission>();
            foreach (var item in permissionGroupCreateModel.ResourceRecords)
            {
                var resource = await _resourceRepository.GetOneAsync(x => x.Id == item.ResourceId);
                if (resource == null) throw new ArgumentNullException("Resource invalid");
                var permissionActual = actionSystem.Where(x => item.ActionIds.Contains(x.Id));
                int permissionValue = permissionActual.Sum(x => x.Value);
                permissions.Add(new Permission
                {
                    ResourceId = item.ResourceId,
                    PermissionValue = permissionValue,
                    PermissionGroupId = newPer.Id
                });
            }
            newPer.Permissions = permissions;
            if (permissionGroupCreateModel.AccountIds != null && permissionGroupCreateModel.AccountIds.Count > 0)
            {
                var accs = await _accountRepository.GetManyAsync(x => permissionGroupCreateModel.AccountIds.Contains(x.Id), "PermissionGroups"); ;
                newPer.Accounts = accs;
            }

            await _permissionGroupRepository.AddAsync(newPer);
            var rs = await _uow.SaveChangeAsync();
            await InvalidPermissionCache();
            return rs > 0;
        }

        public async Task<bool> CreatePermissionGroupValue(PermissionGroupCreateValueModel permissionGroupCreateValueModel)
        {
            var roleCheck = await _permissionGroupRepository.GetManyAsync(x => x.Name.ToLower() == permissionGroupCreateValueModel.Name.ToLower());
            if (roleCheck.Any()) throw new Exception("Permission group name already exist");
            var newPer = new PermissionGroup
            {
                Id = Guid.NewGuid(),
                Name = permissionGroupCreateValueModel.Name
            };
            var actionSystem = await _actionRepository.GetManyAsync();
            List<Permission> permissions = new List<Permission>();
            foreach (var item in permissionGroupCreateValueModel.ResourceRecords)
            {
                var resource = await _resourceRepository.GetOneAsync(x => x.Id == item.ResourceId);
                if (resource == null) throw new ArgumentNullException("Resource invalid");
                int permissionValue = item.PermissionValue;
                permissions.Add(new Permission
                {
                    ResourceId = item.ResourceId,
                    PermissionValue = permissionValue,
                    PermissionGroupId = newPer.Id
                });
            }
            newPer.Permissions = permissions;
            if (permissionGroupCreateValueModel.AccountIds != null && permissionGroupCreateValueModel.AccountIds.Count > 0)
            {
                var accs = await _accountRepository.GetManyAsync(x => permissionGroupCreateValueModel.AccountIds.Contains(x.Id), "PermissionGroups"); ;
                newPer.Accounts = accs;
            }

            await _permissionGroupRepository.AddAsync(newPer);
            var rs = await _uow.SaveChangeAsync();
            await InvalidPermissionCache();
            return rs > 0;
        }

        public async Task<bool> UpdatePermissionActionValue(UpdatePermissionActionValueModel updatePermissionActionValueModel)
        {
            var permissionRoles = await _permissionRepository.GetManyAsync(x => x.PermissionGroupId == updatePermissionActionValueModel.PermissionGroupId);
            List<Permission> permissionList = new List<Permission>();
            foreach (var item in updatePermissionActionValueModel.ResourceValueRecords)
            {
                var permissionRole = permissionRoles.Where(x => x.ResourceId == item.ResourceId).FirstOrDefault()!;
                if (permissionRole != null)
                {
                    permissionRole.PermissionValue = item.PermissionValue;
                    permissionList.Add(permissionRole);
                }
            }
            _permissionRepository.UpdateRange(permissionList);
            var rs = await _uow.SaveChangeAsync();
            await InvalidPermissionCache();
            return rs > 0;
        }

        public async Task<bool> UpdatePermissionGroupOfAccount(UpdateUserPermisisonGroupModel updateUserPermisisonGroupModel)
        {
            var permissionGrCheck = new List<PermissionGroup>();
            if (updateUserPermisisonGroupModel.PermissionGroupIds != null && updateUserPermisisonGroupModel.PermissionGroupIds.Count > 0)
            {
                permissionGrCheck = await _permissionGroupRepository.GetManyAsync(x => updateUserPermisisonGroupModel.PermissionGroupIds.Contains(x.Id));
                if (permissionGrCheck.Any() is false) throw new ArgumentNullException("Not found role");
            }

            var accountCheck = await _accountRepository.GetOneAsync(x => x.Id == updateUserPermisisonGroupModel.AccountId, "PermissionGroups");
            if (accountCheck == null) throw new ArgumentNullException("Not found user");

            if (updateUserPermisisonGroupModel.PermissionGroupIds == null || updateUserPermisisonGroupModel.PermissionGroupIds.Count <= 0)
            {
                if (accountCheck.PermissionGroups != null)
                {
                    accountCheck.PermissionGroups.Clear();
                }
            }
            else
            {
                accountCheck.PermissionGroups = permissionGrCheck;
            }
            _accountRepository.Update(accountCheck);
            var rs = await _uow.SaveChangeAsync();
            await InvalidPermissionGroup(updateUserPermisisonGroupModel.AccountId);
            await InvalidPermissionCache();
            return rs > 0;
        }

        public async Task<List<ActionViewModel>> GetActions(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            if (!string.IsNullOrEmpty((name + "").Trim()))
            {
                return AutoMapperConfig.Mapper.Map<List<ActionViewModel>>(await _actionRepository.GetManyAsync(x => x.Name.ToLower().Contains(name!.ToLower()), pageIndex: pageIndex, pageSize: pageSize, orderBy: x => x.OrderBy(x => x.Index)));
            }
            return AutoMapperConfig.Mapper.Map<List<ActionViewModel>>(await _actionRepository.GetManyAsync(pageIndex: pageIndex, pageSize: pageSize, orderBy: x => x.OrderBy(x => x.Index)));
        }

        public async Task<List<PermissionViewModel>> GetPermissions(int? pageIndex = null, int? pageSize = null)
        {
            return AutoMapperConfig.Mapper.Map<List<PermissionViewModel>>(await _permissionRepository.GetManyAsync(pageIndex: pageIndex, pageSize: pageSize, include: "PermissionGroup,Resource"));
        }

        public async Task<List<ResourceViewModel>> GetResources(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            if (!string.IsNullOrEmpty((name + "").Trim()))
            {
                return AutoMapperConfig.Mapper.Map<List<ResourceViewModel>>(await _resourceRepository.GetManyAsync(x => x.Name.ToLower().Contains(name!.ToLower()), pageIndex: pageIndex, pageSize: pageSize));
            }
            return AutoMapperConfig.Mapper.Map<List<ResourceViewModel>>(await _resourceRepository.GetManyAsync(pageIndex: pageIndex, pageSize: pageSize));
        }

        public async Task<List<PermisisonGroupViewModel>> GetPermisisonGroups(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            if (!string.IsNullOrEmpty((name + "").Trim()))
            {
                return AutoMapperConfig.Mapper.Map<List<PermisisonGroupViewModel>>(await _permissionGroupRepository.GetManyAsync(x => x.Name.ToLower().Contains(name!.ToLower()), pageIndex: pageIndex, pageSize: pageSize));
            }
            return AutoMapperConfig.Mapper.Map<List<PermisisonGroupViewModel>>(await _permissionGroupRepository.GetManyAsync(pageIndex: pageIndex, pageSize: pageSize));
        }
    }
}
