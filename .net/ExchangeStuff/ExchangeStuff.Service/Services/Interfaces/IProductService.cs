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
        Task<ProductViewModel> GetDetail(Guid id);
        Task<bool> updateStatusProduct(UpdateProductViewModel updateProductViewModel);
        Task<List<ProductUserViewModel>> GetProductUser();
        Task<List<ProductUserViewModel>> GetOtherUserProducts(Guid userId);
<<<<<<< HEAD
        Task<List<ProductViewModel>> GetListProductsForAdmin();
        Task<List<ProductViewModel>> GetListProductsForModerator();
=======
        Task<List<ProductViewModel>> GetListProductsForModerator();
        Task<List<ProductViewModel>> GetListProductsForAdmin();
        Task<List<ProductViewModel>> GetProductByName(string name);
>>>>>>> d52b1c07313ee2ea0f6f0aeec2e61d4242bc8331
    }
}
