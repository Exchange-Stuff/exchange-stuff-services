using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls
{
    public class IdentityUser<T> : IIdentityUser<T>
    {
        private readonly ITokenService _tokenService;

        public IdentityUser(ITokenService tokenService)
        {
            _tokenService = tokenService;
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
