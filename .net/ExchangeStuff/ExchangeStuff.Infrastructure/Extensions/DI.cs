using ExchangeStuff.Application;
using ExchangeStuff.Application.Services;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Infrastructure.Persistents;
using ExchangeStuff.Infrastructure.Services;
using ExchangeStuff.Infrastructure.Uows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeStuff.Infrastructure.Extensions
{
    public static class DI
    {
        public static void Inject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExchangeStuffContext>(x => x.UseSqlServer("TODO"));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(ApplicationMediatr).Assembly));
        }
    }
}
