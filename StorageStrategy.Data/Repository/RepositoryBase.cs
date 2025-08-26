using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;

namespace StorageStrategy.Data.Repository
{
    public abstract class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : class
    {
        protected readonly StorageDbContext _context;

        public RepositoryBase(StorageDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TModel model) => await _context.AddAsync(model);
        public async Task<List<TModel>> GetAllAsync(Func<TModel, bool> filter, string[] includeProperties = null)
        {
            IQueryable<TModel> query = _context.Set<TModel>();

            if (includeProperties is not null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await Task.FromResult(query.AsNoTracking().Where(filter).ToList());
        }

        public void Delete(TModel model) => _context.Remove(model);
        public void RemoveRange(TModel model) => _context.RemoveRange(model);
        public abstract Task<TModel> GetById(int id);
        public void Update(TModel model)
        {
            _context.ChangeTracker.Clear();
            _context.Update(model);
        }
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public void Save() => _context.SaveChanges();
        public async Task CreateTranscationAsync() => await _context.Database.BeginTransactionAsync();
        public async Task RollbackAsync() => await _context.Database.RollbackTransactionAsync();
        public async Task CommitAsync() => await _context.Database.CommitTransactionAsync();
        public void Clear() => _context.ChangeTracker.Clear();
    }
}
