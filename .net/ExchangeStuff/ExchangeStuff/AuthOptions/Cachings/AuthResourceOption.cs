using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace ExchangeStuff.AuthOptions.Cachings
{
    public class AuthResourceOption
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        private readonly RedisConstantDTO _redisConstantDTO = new();
        public AuthResourceOption(RequestDelegate requestDelegate, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _requestDelegate = requestDelegate;
            _distributedCache = distributedCache;
            _configuration = configuration;
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisConstantDTO);
        }

        public async Task Invoke(HttpContext context, IAdminService adminService)
        {
            var permission = await _distributedCache.GetStringAsync(_redisConstantDTO.PermissionResource);
            if (permission == null)
            {
                await adminService.SavePermissionCache();
            }

            var actions = await _distributedCache.GetStringAsync(_redisConstantDTO.ActionsResource);
            if (actions == null!)
            {
                await adminService.SaveActionsCache();
            }

            await _requestDelegate(context);
        }
    }
}
