using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ExchangeStuff.Service.Services.Impls
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IPermissionRepository _permissionRepository;
        private readonly RedisDTO _redisDTO = new();
        private readonly RedisConstantDTO _redisConstantDTO = new RedisConstantDTO();

        public PermissionService(IUnitOfWork unitOfWork, IConfiguration configuration, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _connectionMutiple = connectionMultiplexer;
            _permissionRepository = _uow.PermissionRepository;
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisConstantDTO);
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
    }
}
