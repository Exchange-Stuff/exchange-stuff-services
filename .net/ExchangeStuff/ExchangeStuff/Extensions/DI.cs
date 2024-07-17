using ExchangeStuff.AuthOptions.Cachings;
using ExchangeStuff.AuthOptions.Jwts;
using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.AuthOptions.SwaggerOptions;
using ExchangeStuff.Repository;
using ExchangeStuff.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Extensions
{
    public static class DI
    {
        public static void InjectAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR();
            services.AddSwaggerES(configuration);
            services.AddESJwt(configuration);
            services.AddTransient<IAuthorizationPolicyProvider, ESPolicy>();
            services.AddTransient<IAuthorizationHandler, ActionRequirementHandler>();
            services.AddDbContext<ExchangeStuffContext>(x => x.UseSqlServer(GetConnectionString()));
            services.AddScoped<Func<ExchangeStuffContext>>(x => () => x.GetService<ExchangeStuffContext>()!);
            services.AddScoped<DbFactory>();

        }
        private static string GetConnectionString()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", true, true)
                  .Build();
            return config.GetConnectionString("ExchangeStuffDb")!;
        }
        public static void UseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtOption>();
            app.UseMiddleware<AuthResourceOption>();
        }

        public static void UseException(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}
