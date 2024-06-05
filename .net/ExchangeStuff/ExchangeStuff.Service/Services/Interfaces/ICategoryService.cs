using ExchangeStuff.Service.Models.Categories;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> GetCategory(Guid id);
    }
}
