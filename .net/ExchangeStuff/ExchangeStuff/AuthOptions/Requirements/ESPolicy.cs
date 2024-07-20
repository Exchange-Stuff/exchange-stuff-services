using ExchangeStuff.AuthModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ExchangeStuff.AuthOptions.Requirements
{
    public class ESPolicy : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
        public ESPolicy(IOptions<AuthorizationOptions> options)
        {
            options.Value.InvokeHandlersAfterFailure = false;
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
             => FallbackPolicyProvider.GetFallbackPolicyAsync();


        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (string.IsNullOrWhiteSpace(policyName))
            {
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
            var policyAuth = policyName.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (policyAuth.Any() is false) return FallbackPolicyProvider.GetPolicyAsync(policyName);

            var policyScheme = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
            foreach (var item in policyAuth)
            {
                var policyDetail = item.Split('$', StringSplitOptions.RemoveEmptyEntries);
                if (policyDetail != null && policyDetail.Count() == 2)
                {
                    IAuthorizationRequirement authorizationRequirement = policyDetail[0] switch
                    {
                        ESAuthorizeAttribute.ActionGroup => new ActionRequirement(policyDetail[1]),
                        _ => null!
                    };
                    if (authorizationRequirement == null!) return FallbackPolicyProvider.GetPolicyAsync(policyName);
                    policyScheme.AddRequirements(authorizationRequirement);
                }
            }
            return Task.FromResult(policyScheme.Build())!;
        }
    }
}
