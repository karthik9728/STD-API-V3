using MaxiShop.Application.DTO.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interface
{
    public interface IBrandService
    {
        Task<BrandDto> GetById(int id);

        Task<IEnumerable<BrandDto>> GetAll();

        Task<BrandDto> Create(CreateBrandDto createBrandDto);

        Task Update(UpdateBrandDto updateBrandDto);

        Task Delete(int id);
      
    }
}
