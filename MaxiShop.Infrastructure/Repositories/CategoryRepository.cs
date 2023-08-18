using MaxiShop.Domain.Models;
using MaxiShop.Domain.Repositories;
using MaxiShop.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) :  base (dbContext)
        {
            
        }

        public async Task Update(Category productCategory)
        {
            _dbContext.Update(productCategory);
            await _dbContext.SaveChangesAsync();
        }
    }
}
