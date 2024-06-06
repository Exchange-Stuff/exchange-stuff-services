using ExchangeStuff.Service.DTOs;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IActionService
    {
        Task<List<ActionDTO>> GetActionDTOsCache();
        Task SaveActionsCache();
        Task InvalidActionCache();
    }
}
