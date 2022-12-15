using System.Linq.Expressions;

namespace StorageStrategy.Domain.Repository
{
    public interface IRepositoryBase<TModel> where TModel : class
    {
        public Task AddAsync(TModel model);
        public void Update(TModel model);
        public void Delete(int id);
        public TModel GetById(int id);
        public Task<TModel> GetByIdAsync(Expression<Func<TModel, bool>> func);
        public void Save();
        public Task SaveAsync();
    }
}
