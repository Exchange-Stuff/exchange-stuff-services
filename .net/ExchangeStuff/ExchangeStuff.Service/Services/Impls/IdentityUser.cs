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

        public T AccountId
        {
            get
            {
                var claim = _tokenService.GetClaimDTOByAccessTokenSynchronous();

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
