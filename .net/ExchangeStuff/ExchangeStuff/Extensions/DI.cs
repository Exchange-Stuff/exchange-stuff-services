using ExchangeStuff.AuthOptions.Cachings;
using ExchangeStuff.AuthOptions.Jwts;
using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.AuthOptions.SwaggerOptions;
using Microsoft.AspNetCore.Authorization;

namespace ExchangeStuff.Extensions
{
    public static class DI
    {
        public static void InjectAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerES(configuration);
            services.AddESJwt(configuration);
            services.AddTransient<IAuthorizationPolicyProvider, ESPolicy>();
            services.AddTransient<IAuthorizationHandler, ActionRequirementHandler>();
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
