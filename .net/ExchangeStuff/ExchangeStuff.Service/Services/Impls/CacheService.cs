using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Cryptography;

namespace ExchangeStuff.Service.Services.Impls
{
    public class CacheService : ICacheService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private RedisDTO _redisDTO = new();
        private RedisConstantDTO _redisConstantDTO = new();

        public CacheService()
        {

        }
        public CacheService(IUnitOfWork unitOfWork, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IConfiguration configuration)
        {
            _configuration = configuration;
            _uow = unitOfWork;
            _distributedCache = distributedCache;
            _connectionMutiple = connectionMultiplexer;
            _tokenRepository = _uow.TokenRepository;
            _userRepository = _uow.UserRepository;
            _accountRepository = _uow.AccountRepository;
            _configuration.GetSection(nameof(RedisConstantDTO)).Bind(_redisConstantDTO);
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
        }

        public async Task<bool> DeleteAccessToken(string token)
        {
            await _distributedCache.RemoveAsync(token);
            var tokenGet = (await _tokenRepository.GetManyAsync(x => x.AccessToken == token)).FirstOrDefault();
            if (tokenGet == null) throw new UnauthorizedAccessException("Login session has expired");
            _tokenRepository.Remove(tokenGet);
            var rs = await _uow.SaveChangeAsync();
            return rs > 0;
        }

        public async Task<List<PermissionGroupDTO>> GetPermissionGroupByAccountId(Guid id)
        {
            List<PermissionGroup> permissionGroups = new List<PermissionGroup>();
            try
            {
                if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
                {
                    string accountPermissionGroup = (await _distributedCache.GetStringAsync(_redisConstantDTO.PermissionGroupResource + id))!;
                    if (accountPermissionGroup == null!)
                    {
                        var account = await _accountRepository.GetOneAsync(x => x.Id == id, "PermissionGroups");
                        await SavePermissionGroup(id);
                        return AutoMapperConfig.Mapper.Map<List<PermissionGroupDTO>>(account.PermissionGroups);
                    }
                    return (JsonConvert.DeserializeObject<List<PermissionGroupDTO>>(accountPermissionGroup))!;
                }
                var account1 = await _accountRepository.GetOneAsync(x => x.Id == id, "PermissionGroups");
                return AutoMapperConfig.Mapper.Map<List<PermissionGroupDTO>>(account1.PermissionGroups);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> GetTokenValid(string token)
        {
            try
            {
                if (_connectionMutiple.GetServer(_redisDTO.Address, _redisDTO.Port).IsConnected)
                {
                    return (await _distributedCache.GetStringAsync(token) + "") != "";
                }
                return ((await _tokenRepository.GetManyAsync(x => x.AccessToken == token)).FirstOrDefault() + "") != "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InvalidPermissionGroup(Guid id)
        {
            var account = await _accountRepository.GetOneAsync(x => x.Id == id);
            if (account == null!) throw new Exception("Account not found");
            await _distributedCache.RemoveAsync(_redisConstantDTO.PermissionGroupResource + account.Id);
        }

        protected string GenerateRefreshToken()
        {
            var rdr = new byte[32];
            using (var rd = RandomNumberGenerator.Create())
            {
                rd.GetBytes(rdr);
                return Convert.ToBase64String(rdr);
            }
        }

        public async Task<ExchangeStuff.Core.Entities.Token> SaveAccessToken(string token, Guid accountId)
        {
            await _distributedCache.SetStringAsync(token, accountId + "");
            ExchangeStuff.Core.Entities.Token newtk = new ExchangeStuff.Core.Entities.Token
            {
                Id = Guid.NewGuid(),
                AccessToken = token,
                RefreshToken = GenerateRefreshToken(),
                AccountId = accountId,
                CreatedBy = accountId,
                ModifiedBy = accountId
            };
            await _tokenRepository.AddAsync(newtk);
            await _uow.SaveChangeAsync();
            return newtk;
        }

        public async Task SavePermissionGroup(Guid id)
        {
            var account = await _accountRepository.GetOneAsync(x => x.Id == id, "PermissionGroups");
            if (account == null!) throw new Exception("Account not found");
            await _distributedCache.SetStringAsync(_redisConstantDTO.PermissionGroupResource + account.Id, JsonConvert.SerializeObject(account.PermissionGroups, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));
        }

        public async Task SavePermissionGroupAdmin(Guid id)
        {
            var account = await _accountRepository.GetOneAsync(x => x.Id == id, "PermissionGroups");
            if (account == null!) throw new Exception("Account not found");
            await _distributedCache.SetStringAsync(_redisConstantDTO.PermissionGroupResource + account.Id, JsonConvert.SerializeObject(account.PermissionGroups, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));
        }

        public async Task InvalidAllSession(Guid accId)
        {
            var account = await _accountRepository.GetOneAsync(x => x.Id == accId);
            if (account == null) throw new Exception("Not found this account");
            var tokens = await _tokenRepository.GetManyAsync(x => x.AccountId == accId);
            foreach (var item in tokens)
            {
                await _distributedCache.RemoveAsync(item.AccessToken);
            }
            _tokenRepository.RemoveRange(tokens);
            await _uow.SaveChangeAsync();
        }
    }
}
