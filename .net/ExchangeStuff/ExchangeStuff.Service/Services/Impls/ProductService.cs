using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Products;
using System.Linq;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Service.Services.Impls
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ICategoriesRepository _categoriesRepository;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productRepository = _unitOfWork.ProductRepository;
            _categoriesRepository = _unitOfWork.CategoriesRepository;
        }

        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(await _productRepository.GetManyAsync(orderBy: p => p.OrderBy(p => p.CreatedOn)));
        }


        public async Task<List<ProductViewModel>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            var category = await _categoriesRepository.GetOneAsync(
                c => c.Id == categoryId,
                include: "Products"
            );

            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(category.Products);
            
        }


    }
}
