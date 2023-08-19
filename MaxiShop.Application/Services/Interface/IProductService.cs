using MaxiShop.Application.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interface
{
    public interface IProductService
    {
        Task<ProductDto> GetById(int id);

        Task<IEnumerable<ProductDto>> GetAll();

        Task<ProductDto> Create(CreateProductDto createProductDto);

        Task Update(UpdateProductDto updateProductDto);

        Task Delete(int id);
    }
}
