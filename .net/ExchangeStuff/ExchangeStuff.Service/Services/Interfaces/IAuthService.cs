using ExchangeStuff.Service.Models.Tokens;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenViewModel> GetToken(string param);
        Task<bool> Logout();
    }
}
