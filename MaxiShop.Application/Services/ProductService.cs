using AutoMapper;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.InputModels;
using MaxiShop.Application.Services.Interface;
using MaxiShop.Application.ViewModels;
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
        private readonly IPaginationService<ProductDto, Product> _paginationService;

        public ProductService(IProductRepository productRepo, IMapper mapper, IPaginationService<ProductDto, Product> paginationService)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _paginationService = paginationService;
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

        public async Task<PaginationVM<ProductDto>> GetPagination(PaginationIP paginationInput)
        {
            var source = await _productRepo.GetAllProductWithDetailsAsync();

            var result = _paginationService.GetPagination(source, paginationInput);

            return result;
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
