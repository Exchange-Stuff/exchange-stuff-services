using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls
{
    public class ProductService : IProductService
    {
        /// <summary>
        /// Current user request in to Server
        /// Use: _identityUser.UserId
        /// </summary>
        private readonly IIdentityUser<Guid> _identityUser;

        public ProductService(IIdentityUser<Guid> identity)
        {
            _identityUser = identity;
         
        }
    }
}
