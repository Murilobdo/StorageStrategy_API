using System.Linq.Expressions;

namespace StorageStrategy.Domain.Repository
{
    public interface IRepositoryBase<TModel> where TModel : class
    {
        public Task AddAsync(TModel model);
        public void Update(TModel model);
        public void Delete(TModel id);
        public void Save();
        public Task SaveAsync();
    }
}
