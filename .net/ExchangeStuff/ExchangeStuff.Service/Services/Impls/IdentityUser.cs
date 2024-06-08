using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ExchangeStuff.Service.Services.Impls
{
    public class IdentityUser<T> : TokenService, IIdentityUser<T>
    {
        public IdentityUser(IConfiguration configuration, IHttpContextAccessor httpContext) : base(configuration, httpContext)
        {
        }

        public T AccountId
        {
            get
            {
                ClaimDTO claim = GetClaimDTOByAccessTokenSynchronous();
                Guid accId = Guid.Empty;
                if (claim != null!)
                {
                    accId = claim.Id;
                }
                return (T)Convert.ChangeType(accId, typeof(T));
            }
        }
    }
}
