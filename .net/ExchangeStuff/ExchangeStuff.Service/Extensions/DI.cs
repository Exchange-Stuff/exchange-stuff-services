using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Repository.Data;
using ExchangeStuff.Repository.Uows;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Library;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ExchangeStuff.Service.Extensions
{
    public static class DI
    {
        public static void Inject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExchangeStuffContext>(x => x.UseSqlServer("TODO"));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRatingSerivce, RatingService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IIdentityUser<Guid>, IdentityUser<Guid>>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
            try
            {
                var redisDto = new RedisDTO();
                configuration.GetSection(nameof(RedisDTO)).Bind(redisDto);
                services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(redisDto.Address));
                services.AddStackExchangeRedisCache(x => x.Configuration = redisDto.Address);
            }
            catch (RedisConnectionException ex)
            {
                throw new Exception(ex.Message);
            }
            services.AddScoped<IVnPayService, VNPayService>();
            services.AddSingleton<VnPayLibrary>();
            services.AddScoped<IPurchaseTicketService, PurchaseTicketService>();
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
