﻿using MaxiShop.Application.DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetById(int id);

        Task<IEnumerable<CategoryDto>> GetAll();

        Task<CategoryDto> Create(CreateCategoryDto createCategoryDto);

        Task Update(UpdateCategoryDto updateCategoryDto);

        Task Delete(int id);
      
    }
}
