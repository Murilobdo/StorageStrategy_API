using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;

namespace StorageStrategy.Data.Repository
{
    public class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : class
    {
        private readonly StorageDbContext _context;

        public RepositoryBase(StorageDbContext context)
        {
            _context = context;
        }
        public void Add(TModel model) => _context.Add(model);
        public void Delete(int id) => _context.Remove(id);
        public void GetById(int id) => _context.Find<TModel>(id);
        public async Task GetByIdAsync(int id) => await _context.FindAsync<TModel>(id);
        public void Update(TModel model) => _context.Update(model);
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public void Save() => _context.SaveChanges();
    }
}
