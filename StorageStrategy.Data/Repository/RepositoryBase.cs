using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using System.Linq.Expressions;

namespace StorageStrategy.Data.Repository
{
    public class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : class
    {
        protected readonly StorageDbContext _context;

        public RepositoryBase(StorageDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TModel model) => await _context.AddAsync(model);
        public void Delete(TModel model) => _context.Remove(model);
        public void RemoveRange(TModel model) => _context.RemoveRange(model);
        public TModel GetById(int id) => _context.Find<TModel>(id);
        public void Update(TModel model) => _context.Update(model);
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public void Save() => _context.SaveChanges();
    }
}
