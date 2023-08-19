using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Domain.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task Update(Brand brand);
    }
}
