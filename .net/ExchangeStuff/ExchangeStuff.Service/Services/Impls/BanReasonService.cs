using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.BanReasons;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace ExchangeStuff.Service.Services.Impls
{
    public class BanReasonService :  IBanReasonService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpConextAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IUnitOfWork _uow;
        private readonly IBanReasonRepository _banReasonRepository;

        public BanReasonService(IConfiguration configuration, IUnitOfWork unitOfWork, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor) 
        {
            _uow = unitOfWork;
            _httpConextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
            _configuration = configuration;
            _banReasonRepository = _uow.BanReasonRepository;
        }

        public async Task<BanReasonViewModel> CreateBanReasons(BanReasonCreateModel banReasonCreateModel)
        {
            if (banReasonCreateModel == null! || string.IsNullOrEmpty(banReasonCreateModel.Content))
            {
                throw new Exception("Content is required");
            }
            var content = await _banReasonRepository.GetManyAsync(x => x.Content.ToLower().Trim() == banReasonCreateModel.Content.ToLower().Trim() && x.BanReasonType == banReasonCreateModel.BanReasonType);

            if (content.Any()) throw new Exception("Ban content already exist");

            BanReason banReason = new BanReason
            {
                Content = banReasonCreateModel.Content,
                BanReasonType=banReasonCreateModel.BanReasonType
            };
            await _banReasonRepository.AddAsync(banReason);
            await _uow.SaveChangeAsync();
            return AutoMapperConfig.Mapper.Map<BanReasonViewModel>(banReason);
        }

        public async Task<BanReasonViewModel> GetBanReason(Guid id)
        => AutoMapperConfig.Mapper.Map<BanReasonViewModel>(await _banReasonRepository.GetOneAsync(x => x.Id == id));

        public async Task<List<BanReasonViewModel>> GetBanReasons(string? content = null, BanReasonType? banReasonType = null!)
        {
            var banrs = await _banReasonRepository.GetManyAsync(x => x.BanReasonType == banReasonType);
            if (!string.IsNullOrEmpty((content + "").Trim()))
            {
                banrs = banrs.Where(x => x.Content.ToLower().Contains(content!.ToLower().Trim())).ToList();
            }
            if (banReasonType.HasValue)
            {
                banrs = banrs.Where(x => x.BanReasonType == banReasonType.Value).ToList();
            }
            return (AutoMapperConfig.Mapper.Map<List<BanReasonViewModel>>(banrs));
        }

        public async Task<BanReasonViewModel> UpdateBanReasons(BanReasonUpdateModel banReasonUpdateModel)
        {
            if (banReasonUpdateModel == null! || string.IsNullOrEmpty(banReasonUpdateModel.Content))
            {
                throw new Exception("Content is required");
            }
            var content = await _banReasonRepository.GetManyAsync(x => x.Content.ToLower().Trim() == banReasonUpdateModel.Content.ToLower().Trim() && x.BanReasonType == banReasonUpdateModel.BanReasonType);

            if (content.Any()) throw new Exception("Ban content already exist");

            var banrs = await _banReasonRepository.GetOneAsync(x => x.Id == banReasonUpdateModel.Id, forUpdate: true);

            banrs.Content = banReasonUpdateModel.Content;
            banrs.BanReasonType = banReasonUpdateModel.BanReasonType;
            _banReasonRepository.Update(banrs);
            await _uow.SaveChangeAsync();
            return AutoMapperConfig.Mapper.Map<BanReasonViewModel>(banrs);
        }
    }
}
