using ExchangeStuff.AuthOptions;
using Microsoft.AspNetCore.Authorization;

namespace ExchangeStuff.Extensions
{
    public static class DI
    {
        public static void InjectAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthorizationPolicyProvider, ESPolicy>();
            services.AddTransient<IAuthorizationHandler, ActionRequirementHandler>();
        }
    }
}
