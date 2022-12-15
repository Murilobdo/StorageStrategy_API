using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using System.Linq.Expressions;

namespace StorageStrategy.Data.Repository
{
    public class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : class
    {
        private readonly StorageDbContext _context;

        public RepositoryBase(StorageDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TModel model) => await _context.AddAsync(model);
        public void Delete(int id) => _context.Remove(id);
        public TModel GetById(int id) => _context.Find<TModel>(id);
        public void Update(TModel model) => _context.Update(model);
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public void Save() => _context.SaveChanges();
        public async Task<TModel> GetByIdAsync(Expression<Func<TModel, bool>> func) =>
            await _context.Set<TModel>().FirstOrDefaultAsync(func);
    }
}
