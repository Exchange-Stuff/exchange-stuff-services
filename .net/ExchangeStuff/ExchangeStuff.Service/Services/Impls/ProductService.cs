﻿using ExchangeStuff.Core.Entities;
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


        public ProductService(IUnitOfWork unitOfWork, IIdentityUser<Guid> identityUser)
        {
            _unitOfWork = unitOfWork;
            _productRepository = _unitOfWork.ProductRepository;
            _categoriesRepository = _unitOfWork.CategoriesRepository;
            _postTicketRepository = _unitOfWork.PostTicketRepository;
            _transactionHistoryRepository = _unitOfWork.TransactionHistoryRepository;
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
                    await createPostTicket(postTicketViewModel);

                    

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
            postTicket.UserId = _identityUser.AccountId;
            await _postTicketRepository.AddAsync(postTicket);

        }

        public async Task<ProductViewModel> GetDetail(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<ProductViewModel>(await _productRepository.GetOneAsync(predicate: p => p.Id == id));
        }




    }
}
