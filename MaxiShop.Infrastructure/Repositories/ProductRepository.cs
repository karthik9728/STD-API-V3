using MaxiShop.Domain.Models;
using MaxiShop.Domain.Repositories;
using MaxiShop.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<Product>> GetAllProductWithDetailsAsync()
        {
            return await _dbContext.Product.Include(x => x.Category).Include(x => x.Brand).ToListAsync();
        }

        public async Task<Product> GetProductDetailsAsync(int id)
        {
            return await _dbContext.Product.Include(x => x.Category).Include(x => x.Brand).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task Update(Product product)
        {
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
