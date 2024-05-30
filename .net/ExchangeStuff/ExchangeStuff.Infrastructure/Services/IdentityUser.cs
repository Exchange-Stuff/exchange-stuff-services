using ExchangeStuff.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ExchangeStuff.Infrastructure.Services
{
    public class IdentityUser<T> : IIdentityUser<T>
    {
        private readonly ITokenService _tokenService;

        public IdentityUser(ITokenService tokenService)
        {
            _tokenService=tokenService;
        }

        public T UserId
        {
            get
            {
                var claim = _tokenService.GetClaimDTOByAccessTokenSynchronous();

                Guid userId = Guid.Empty;
                if (claim != null!)
                {
                    userId = claim.Id;
                }
                return (T)Convert.ChangeType(userId, typeof(T));
            }
        }
    }
}
