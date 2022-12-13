using Microsoft.EntityFrameworkCore;
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

        public async Task<CategoryEntity> FindByName(string name)
        {
            return await _context.Category.FirstOrDefaultAsync(p => p.Name == name);
        }
    }
}
