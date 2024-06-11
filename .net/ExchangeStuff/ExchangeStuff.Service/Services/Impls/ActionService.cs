using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ExchangeStuff.Service.Services.Impls
{
    public class ActionService : IActionService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IDistributedCache _cache;
        private readonly IUnitOfWork _uow;
        private readonly IActionRepository _actionRepository;
        private readonly RedisDTO _redisDTO = new RedisDTO();
        private readonly RedisConstantDTO _redisConstantDTO = new();
        public ActionService(IUnitOfWork unitOfWork, IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache, IHttpContextAccessor httpContext) 
        {
            _configuration = configuration;
            _connectionMutiple = connectionMultiplexer;
            _cache = distributedCache;
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisConstantDTO);
            _uow = unitOfWork;
            _actionRepository = _uow.ActionRepository;
        }
    }
}
 