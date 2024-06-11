using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Categories;
using ExchangeStuff.Service.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Impls
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoriesRepository = _unitOfWork.CategoriesRepository;
           
        }

        public async Task<List<CategoryViewModel>> GetCategory()
        {
            return AutoMapperConfig.Mapper.Map<List<CategoryViewModel>>(await _categoriesRepository.GetManyAsync());
        }
    }
}
