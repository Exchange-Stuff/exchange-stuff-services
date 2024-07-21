using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.UserBanReports;
using ExchangeStuff.Service.Paginations;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace ExchangeStuff.Service.Services.Impls
{
    public class UserBanReportService : IUserBanReportService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpConextAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IBanReasonRepository _banReasonRepository;
        private readonly IUserBanReportRepository _userBanReportRepository;
        private readonly IUserRepository _userRepository;

        public UserBanReportService(IConfiguration configuration, IUnitOfWork unitOfWork, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor, IIdentityUser<Guid> identityUser)
        {
            _uow = unitOfWork;
            _identityUser = identityUser;
            _httpConextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
            _configuration = configuration;
            _banReasonRepository = _uow.BanReasonRepository;
            _userBanReportRepository = _uow.UserBanReportRepository;
            _userRepository = _uow.UserRepository;
        }

        public async Task<bool> ApproveUserBanReport(Guid id)
        {
            var userBanReport = await _userBanReportRepository.GetOneAsync(x => x.Id == id && !x.IsApproved, forUpdate: true);
            userBanReport.IsApproved = true;
            _userBanReportRepository.Update(userBanReport);
            return (await _uow.SaveChangeAsync()) > 0;
        }

        public async Task<UserBanReportViewModel> CreateUserBanReport(UserBanReportCreateModel userBanReportCreateModel)
        {
            if (_identityUser.AccountId == Guid.Empty) throw new UnauthorizedAccessException("Login session expired");

            var user = await _userRepository.GetOneAsync(x => x.Id == _identityUser.AccountId);

            if (user == null) throw new UnauthorizedAccessException("Login session expired");
            var us = await _userRepository.GetOneAsync(x => x.Id == userBanReportCreateModel.UserId);
            if (us == null!) throw new Exception("Not found this user");
            var banrs = await _banReasonRepository.GetOneAsync(x => x.Id == userBanReportCreateModel.BanReasonId && x.BanReasonType == Core.Enums.BanReasonType.User);
            if (banrs == null) throw new Exception("Not found ban reason");
            var uBanReport = await _userBanReportRepository.GetOneAsync(x => x.UserId == userBanReportCreateModel.UserId && x.CreatedBy == _identityUser.AccountId && !x.IsApproved);
            if (uBanReport != null) throw new Exception("This report waiting approve");
            UserBanReport userBanReport = new UserBanReport
            {
                BanReasonId = userBanReportCreateModel.BanReasonId,
                IsApproved = false,
                UserId = userBanReportCreateModel.UserId,
                UserCreateId=_identityUser.AccountId,
            };

            await _userBanReportRepository.AddAsync(userBanReport);
            await _uow.SaveChangeAsync();
            return AutoMapperConfig.Mapper.Map<UserBanReportViewModel>(userBanReport);
        }

        public async Task<UserBanReportViewModel> GetUserBanReport(Guid id)
        => AutoMapperConfig.Mapper.Map<UserBanReportViewModel>(await _userBanReportRepository.GetOneAsync(x => x.Id == id, "User,BanReason"));

        public async Task<PaginationItem<UserBanReportViewModel>> GetUserBanReports(Guid? userId = null!, List<Guid>? reasonIds = null, Guid? reasonId = null, string? reason = null, int? pageIndex = null, int? pageSize = null)
        {
            var userBans = await _userBanReportRepository.GetManyAsync(include: "BanReason,User");
            if (userId.HasValue)
            {
                userBans = userBans.Where(x => x.UserId == userId).ToList();
            }
            if (reasonIds != null && reasonIds.Count > 0)
            {
                var reasonsGet = (await _banReasonRepository.GetManyAsync()).AsEnumerable();
                var reasonIdsGet = reasonsGet.Select(x => x.Id).ToList();
                userBans = userBans.Where(x => reasonIdsGet.Contains(x.BanReasonId)).ToList();
            }
            if (reasonId.HasValue)
            {
                userBans = userBans.Where(x => x.BanReasonId == reasonId).ToList();
            }
            if (!string.IsNullOrEmpty((reason + "").Trim()))
            {
                var reasonsGet1 = (await _banReasonRepository.GetManyAsync(x => x.Content.ToLower().Contains(reason!.ToLower().Trim()))).AsEnumerable();
                var reasonIdsGet1 = reasonsGet1.Select(x => x.Id).ToList();
                userBans = userBans.Where(x => reasonIdsGet1.Contains(x.BanReasonId)).ToList();
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                userBans = userBans.Skip(index * size).Take(size).ToList();
            }
            return PaginationItem<UserBanReportViewModel>.ToPagedList(AutoMapperConfig.Mapper.Map<List<UserBanReportViewModel>>(userBans), pageIndex, pageSize);
        }
    }
}
