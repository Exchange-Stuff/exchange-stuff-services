using ExchangeStuff.Service.Models.Moderators;
using ExchangeStuff.Service.Models.Tokens;
using ExchangeStuff.Service.Services.Impls;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenViewModel> GetToken(string param);
        Task<bool> Logout();
        Task<ModeratorViewModel> CreateModerator(ModeratorCreateModel moderatorCreateModel);
        Task<TokenViewModel> LoginUsernameAndPwd(LoginRd loginRd);
        Task<bool> DeleteAccount(Guid id);
        Task<bool> ValidScreen(string resource);

    }
}
