using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Products;
using System.Linq;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ExchangeStuff.Service.Models.Categories;

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

        public async Task<bool> CreateProductAsync(CreateProductModel model)
        {

            try
            {
                if (model.CategoryId == null || !model.CategoryId.Any())
                {
                    throw new ArgumentException("Category Ids cannot be null or empty");
                }

                var categories = await _unitOfWork.CategoriesRepository.GetManyAsync(c => model.CategoryId.Contains(c.Id));

                var product = AutoMapperConfig.Mapper.Map<Product>(model);
                product.Id = Guid.NewGuid();
                product.IsActived = true;
                product.Categories = categories.ToList();

                await _unitOfWork.ProductRepository.AddAsync(product);

                var result = await _unitOfWork.SaveChangeAsync();

                return result > 0;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error");
            }
        }


        public async Task<ProductViewModel> GetDetail(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<ProductViewModel>(await _productRepository.GetOneAsync(predicate: p => p.Id == id));
        }



    }
}
