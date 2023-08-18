using AutoMapper;
using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.Services.Interface;
using MaxiShop.Domain.Models;
using MaxiShop.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var productCategories = await _categoryRepo.GetAllAsync();

            return _mapper.Map<List<CategoryDto>>(productCategories);
        }

        public async Task<CategoryDto> GetById(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(x => x.Id == id);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> Create(CreateCategoryDto createCategoryDto)
        {

            var category = _mapper.Map<Category>(createCategoryDto);

            var createEntity = await _categoryRepo.CreateAsync(category);

            var entity = _mapper.Map<CategoryDto>(createEntity);

            return entity;

        }

        public async Task Update(UpdateCategoryDto updateCategoryDto)
        {
            var category = _mapper.Map<Category>(updateCategoryDto);

            await _categoryRepo.Update(category);
        }

        public async Task Delete(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(x => x.Id == id);

            await _categoryRepo.DeleteAsync(category);
        }
    }
}
