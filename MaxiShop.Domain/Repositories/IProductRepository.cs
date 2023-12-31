﻿using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Domain.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllProductWithDetailsAsync();

        Task<Product> GetProductDetailsAsync(int id);

        Task Update(Product product);
    }
}
