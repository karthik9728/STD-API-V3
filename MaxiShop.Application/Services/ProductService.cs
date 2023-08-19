using AutoMapper;
using MaxiShop.Application.DTO.Product;
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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var productCategories = await _productRepo.GetAllProductWithDetailsAsync();

            return _mapper.Map<List<ProductDto>>(productCategories);
        }

        public async Task<ProductDto> GetById(int id)
        {
            var product = await _productRepo.GetProductDetailsAsync(id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> Create(CreateProductDto createProductDto)
        {

            var product = _mapper.Map<Product>(createProductDto);

            var createEntity = await _productRepo.CreateAsync(product);

            var entity = _mapper.Map<ProductDto>(createEntity);

            return entity;

        }

        public async Task Update(UpdateProductDto updateProductDto)
        {
            var product = _mapper.Map<Product>(updateProductDto);

            await _productRepo.Update(product);
        }

        public async Task Delete(int id)
        {
            var product = await _productRepo.GetByIdAsync(x => x.Id == id);

            await _productRepo.DeleteAsync(product);
        }
    }
}
