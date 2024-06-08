namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetToken(string authCode);
        Task<bool> Logout();
    }
}
