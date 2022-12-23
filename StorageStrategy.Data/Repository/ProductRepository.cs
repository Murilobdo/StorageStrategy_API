using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using System.ComponentModel.Design;

namespace StorageStrategy.Data.Repository
{
    public class ProductRepository : RepositoryBase<ProductEntity>, IProductRepository
    {
        private StorageDbContext _context;

        public ProductRepository(StorageDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ProductEntity> FindByName(string name, int companyId)
        {
            return await _context.Product.FirstOrDefaultAsync(p => p.Name == name && p.CompanyId == companyId);
        }

        public async Task<ProductEntity> GetByIdAsync(int categoryId, int companyId)
        {
            var result = await _context.Product.FirstOrDefaultAsync(p => p.CategoryId == categoryId && p.CompanyId == companyId);
            _context.ChangeTracker.Clear();
            return result;
        }

        public async Task<List<ProductEntity>> ToList(int companyId)
        {
            return await _context.Product.Where(p => p.CompanyId == companyId).ToListAsync();
        }
    }
}
