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

        public async Task<List<ActionDTO>> GetActionDTOsCache()
        {
            if (_redisDTO == null!) throw new ArgumentNullException("Can't bind redis server");
            if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
            {
                var actionString = await _cache.GetStringAsync(_redisConstantDTO.ActionsResource);

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
            await _cache.RemoveAsync(_redisConstantDTO.ActionsResource);
        }
        public async Task SaveActionsCache()
        {
            var actions = await _actionRepository.GetManyAsync();
            if (actions == null! || actions.Count == 0) throw new ArgumentNullException("Not found user authorize");
            await _cache.SetStringAsync(_redisConstantDTO.ActionsResource, JsonConvert.SerializeObject(AutoMapperConfig.Mapper.Map<List<ActionDTO>>(actions)));
        }
    }
}
