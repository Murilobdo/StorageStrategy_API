﻿using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository
{
    public class CategoryRepository : RepositoryBase<CategoryEntity>, ICategoryRepository
    {
        private StorageDbContext _context;
        
        public CategoryRepository(StorageDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CategoryEntity> FindByName(string name, int companyId)
        {
            return await _context.Category.FirstOrDefaultAsync(p => p.Name == name && p.CompanyId == companyId);
        }

        public async Task<CategoryEntity> GetByIdAsync(int categoryId, int companyId)
        {
            var result = await _context.Category.FirstOrDefaultAsync(p => p.CategoryId == categoryId && p.CompanyId == companyId);
            _context.ChangeTracker.Clear();
            return result;
        }

        public async Task<List<CategoryEntity>> ToList(int companyId)
        {
            return await _context.Category.Where(p => p.CompanyId == companyId).ToListAsync();
        }
    }
}
