using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.Categories;
using ExchangeStuff.Service.Models.Products;

namespace ExchangeStuff.Service.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetAllProductsAsync();
        Task<List<ProductViewModel>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<bool> CreateProductAsync(CreateProductModel model);
        Task<ProductImageUserViewModel> GetDetail(Guid id);
        Task<bool> updateStatusProduct(UpdateProductViewModel updateProductViewModel);
    }
}
