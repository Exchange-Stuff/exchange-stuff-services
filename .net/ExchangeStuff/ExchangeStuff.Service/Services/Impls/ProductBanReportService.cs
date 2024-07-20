using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.ProductBanReports;
using ExchangeStuff.Service.Models.UserBanReports;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace ExchangeStuff.Service.Services.Impls
{
    public class ProductBanReportService : IProductBanReportService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpConextAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IBanReasonRepository _banReasonRepository;
        private readonly IProductBanReportRepository _productBanReportRepository;
        private readonly IUserRepository _userRepository;

        public ProductBanReportService(IConfiguration configuration, IUnitOfWork unitOfWork, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor, IIdentityUser<Guid> identityUser)
        {
            _uow = unitOfWork;
            _identityUser = identityUser;
            _httpConextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
            _configuration = configuration;
            _banReasonRepository = _uow.BanReasonRepository;
            _productBanReportRepository = _uow.ProductBanReportRepository;
            _userRepository = _uow.UserRepository;
        }

        public async Task<bool> ApproveProductBanReport(Guid id)
        {
            var productBanrp = await _productBanReportRepository.GetOneAsync(x => x.Id == id && !x.IsApproved, forUpdate: true);
            productBanrp.IsApproved = true;
            _productBanReportRepository.Update(productBanrp);
            return (await _uow.SaveChangeAsync()) > 0;
        }

        public async Task<ProductBanReportViewModel> CreateProductBanReport(ProductBanReportCreateModel productBanReportCreateModel)
        {
            if (_identityUser.AccountId == Guid.Empty) throw new UnauthorizedAccessException("Login session expired");

            var user = await _userRepository.GetOneAsync(x => x.Id == _identityUser.AccountId);

            if (user == null) throw new UnauthorizedAccessException("Login session expired");

            var productBanCheck = await _productBanReportRepository.GetOneAsync(x => x.ProductId == productBanReportCreateModel.ProductId && x.CreatedBy == _identityUser.AccountId && !x.IsApproved);
            if (productBanCheck != null) throw new Exception("This report waiting accept");
            var banrs = await _banReasonRepository.GetOneAsync(x => x.Id == productBanReportCreateModel.BanReasonId && x.BanReasonType == Core.Enums.BanReasonType.Product);
            if (banrs == null) throw new Exception("Not found ban reason");
            ProductBanReport productBanReport = new ProductBanReport
            {
                BanReasonId = productBanReportCreateModel.BanReasonId,
                IsApproved = false,
                ProductId = productBanReportCreateModel.ProductId,
                UserId = _identityUser.AccountId,
            };
            await _productBanReportRepository.AddAsync(productBanReport);
            await _uow.SaveChangeAsync();
            return AutoMapperConfig.Mapper.Map<ProductBanReportViewModel>(productBanReport);
        }

        public async Task<ProductBanReportViewModel> GetProductBanReport(Guid id)
        => AutoMapperConfig.Mapper.Map<ProductBanReportViewModel>(await _productBanReportRepository.GetOneAsync(x => x.Id == id));

        public async Task<PaginationItem<ProductBanReportViewModel>> GetProductBanReports(Guid? productId = null, List<Guid>? reasonIds = null, Guid? reasonId = null, string? reason = null, int? pageIndex = null, int? pageSize = null)
        {
            var productBans = await _productBanReportRepository.GetManyAsync(include: "BanReason,Product");
            if (productId.HasValue)
            {
                productBans = productBans.Where(x => x.ProductId == productId).ToList();
            }
            if (reasonIds != null && reasonIds.Count > 0)
            {
                var reasonsGet = (await _banReasonRepository.GetManyAsync()).AsEnumerable();
                var reasonIdsGet = reasonsGet.Select(x => x.Id).ToList();
                productBans = productBans.Where(x => reasonIdsGet.Contains(x.BanReasonId)).ToList();
            }
            if (reasonId.HasValue)
            {
                productBans = productBans.Where(x => x.BanReasonId == reasonId).ToList();
            }
            if (!string.IsNullOrEmpty((reason + "").Trim()))
            {
                var reasonsGet1 = (await _banReasonRepository.GetManyAsync(x => x.Content.ToLower().Contains(reason!.ToLower().Trim()))).AsEnumerable();
                var reasonIdsGet1 = reasonsGet1.Select(x => x.Id).ToList();
                productBans = productBans.Where(x => reasonIdsGet1.Contains(x.BanReasonId)).ToList();
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                productBans = productBans.Skip(index * size).Take(size).ToList();
            }
            return PaginationItem<ProductBanReportViewModel>.ToPagedList(AutoMapperConfig.Mapper.Map<List<ProductBanReportViewModel>>(productBans),pageIndex,pageSize);
        }
    }
}
