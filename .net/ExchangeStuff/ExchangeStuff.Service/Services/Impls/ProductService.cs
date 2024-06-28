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
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Service.Models.PostTicket;
using ExchangeStuff.Repository.Repositories;
using ExchangeStuff.Service.Models.Users;

namespace ExchangeStuff.Service.Services.Impls
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IPostTicketRepository _postTicketRepository;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;
        private readonly IUserBalanceRepository _userBalanceRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(IUnitOfWork unitOfWork, IIdentityUser<Guid> identityUser)
        {
            _unitOfWork = unitOfWork;
            _productRepository = _unitOfWork.ProductRepository;
            _categoriesRepository = _unitOfWork.CategoriesRepository;
            _postTicketRepository = _unitOfWork.PostTicketRepository;
            _transactionHistoryRepository = _unitOfWork.TransactionHistoryRepository;
            _userBalanceRepository = _unitOfWork.UserBalanceRepository;
            _userRepository = _unitOfWork.UserRepository;
            _identityUser = identityUser;
        }

        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {

            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(await _productRepository.GetManyAsync(predicate: p => p.ProductStatus.Equals(ProductStatus.Approve), orderBy: p => p.OrderBy(p => p.CreatedOn)));
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
                product.IsActived = false;
                product.Categories = categories.ToList();
                product.ProductStatus = ProductStatus.Pending;

                List<Image> images = new List<Image>();
                foreach (var item in model.ImageUrls)
                {
                    images.Add(new Image
                    {
                        Url = item
                    });
                }
                product.Images = images;

                await _unitOfWork.ProductRepository.AddAsync(product);

                var result = await _unitOfWork.SaveChangeAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> updateStatusProduct(UpdateProductViewModel updateProductViewModel)
        {
            try
            {
                var product = await _productRepository.GetOneAsync(predicate: p => p.Id.Equals(updateProductViewModel.Id));

                if (product == null) 
                {
                    throw new Exception("Not found product");
                }

                product.ProductStatus = updateProductViewModel.ProductStatus;

                _productRepository.Update(product);

                if (product.ProductStatus.Equals(ProductStatus.Approve)) 
                {
                    PostTicketViewModel postTicketViewModel = new PostTicketViewModel();
                    postTicketViewModel.productId = product.Id;
                    postTicketViewModel.Amount = 10;
                    postTicketViewModel.UserId = product.CreatedBy;
                    await createPostTicket(postTicketViewModel);

                    var userBl = await _userBalanceRepository.GetOneAsync(predicate: p => p.UserId.Equals(product.CreatedBy));

                    if (userBl != null) 
                    {
                        if (userBl.Balance < 0)
                        {
                            throw new Exception("Not enough money");
                        }
                        else 
                        {
                            userBl.Balance = userBl.Balance - 10;
                        }
                    }

                }
                
                var result = await _unitOfWork.SaveChangeAsync();

                return result > 0;

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task createPostTicket(PostTicketViewModel postTicketViewModel)
        {
            var postTicket = AutoMapperConfig.Mapper.Map<PostTicket>(postTicketViewModel);
            postTicket.Id = Guid.NewGuid();
            postTicket.Status = PostTicketStatus.Approve;

            await _postTicketRepository.AddAsync(postTicket);

        }

        public async Task<ProductViewModel> GetDetail(Guid id)
        {
            var product = await _productRepository.GetOneAsync(predicate: p => p.Id == id, include: "Images");
            if (product == null) throw new Exception("Not found product!");
            var result = AutoMapperConfig.Mapper.Map<ProductViewModel>(product);
            return result;
        }

        public async Task<List<ProductUserViewModel>> GetProductUser()
        {
            var product = await _productRepository.GetManyAsync(predicate: p => p.CreatedBy.Equals(_identityUser.AccountId));

            if (product == null) throw new Exception("Not found product!");

            return AutoMapperConfig.Mapper.Map<List<ProductUserViewModel>>(product);

        }

        public async Task<List<ProductUserViewModel>> GetOtherUserProducts(Guid userId)
        {
            var product = await _productRepository.GetManyAsync(predicate: p => p.CreatedBy.Equals(userId) && p.IsActived && p.ProductStatus == ProductStatus.Approve);

            if (product == null) throw new Exception("Not found product!");

            return AutoMapperConfig.Mapper.Map<List<ProductUserViewModel>>(product);
        }
    }
}
