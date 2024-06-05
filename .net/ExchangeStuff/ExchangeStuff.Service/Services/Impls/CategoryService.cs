using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Categories;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _categoryRepository = _uow.CategoryRepository;
        }

        public async Task<CategoryViewModel> GetCategory(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<CategoryViewModel>(await _categoryRepository.GetOneAsync(x => x.Id == id));
        }
    }
}
