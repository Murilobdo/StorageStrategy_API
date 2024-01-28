﻿using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository
{
    public class ProductRepository : RepositoryBase<ProductEntity>, IProductRepository
    {
        private StorageDbContext _context;

        public ProductRepository(StorageDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRange(List<ProductEntity> products)
        {
            await _context.Product.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductEntity> FindByName(string name, int companyId)
        {
            return await _context.Product.FirstOrDefaultAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim() && p.CompanyId == companyId);
        }

        public override async Task<ProductEntity> GetById(int id)
        {
            return await _context.Product.FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<ProductEntity> GetByIdAsync(int productId, int companyId)
        {
            var result = await _context.Product.FirstOrDefaultAsync(p => p.ProductId == productId && p.CompanyId == companyId);
            _context.ChangeTracker.Clear();
            return result;
        }

        public async Task<int> QuantityInStockByCompany(int companyId)
        {
            int result = await _context.Product
                                    .Where(p => p.CompanyId == companyId)
                                    .SumAsync(p => p.Qtd);
            return result;
        }

        public async Task<decimal> ReadTotalPriceInStokByCompany(int companyId)
        {
            decimal result = await _context.Product
                                    .Where(p => p.CompanyId == companyId)
                                    .SumAsync(p => p.Price * p.Qtd);
            return result;
        }

        public async Task<List<ProductEntity>> ToList(int companyId, bool active)
        {
            var query = _context.Product
                .Where(p => p.CompanyId == companyId);

            if (active)
                query = query.Where(p => p.Qtd > 0);

            return await query.ToListAsync();
        }
    }
}
