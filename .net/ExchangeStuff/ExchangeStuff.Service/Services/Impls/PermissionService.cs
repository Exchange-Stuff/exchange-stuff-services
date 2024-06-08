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

      
    }
}
