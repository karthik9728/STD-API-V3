using AutoMapper;
using MaxiShop.Application.DTO.Brand;
using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.Exceptions;
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
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDto>> GetAll()
        {
            var brands = await _brandRepo.GetAllAsync();

            return _mapper.Map<List<BrandDto>>(brands);
        }

        public async Task<BrandDto> GetById(int id)
        {
            var brand = await _brandRepo.GetByIdAsync(x => x.Id == id);

            return _mapper.Map<BrandDto>(brand);
        }

        public async Task<BrandDto> Create(CreateBrandDto createBrandDto)
        {

            //Validate incoming data
            var validator = new CreateBrandDtoValidator();

            var validationResult = await validator.ValidateAsync(createBrandDto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid Brand Input", validationResult);
            }

            var brand = _mapper.Map<Brand>(createBrandDto);

            var createEntity = await _brandRepo.CreateAsync(brand);

            var entity = _mapper.Map<BrandDto>(createEntity);

            return entity;

        }

        public async Task Update(UpdateBrandDto updateBrandDto)
        {
            var brand = _mapper.Map<Brand>(updateBrandDto);

            await _brandRepo.Update(brand);
        }

        public async Task Delete(int id)
        {
            var category = await _brandRepo.GetByIdAsync(x => x.Id == id);

            await _brandRepo.DeleteAsync(category);
        }
    }
}
