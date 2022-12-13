namespace StorageStrategy.Domain.Repository
{
    public interface IRepositoryBase<TModel> where TModel : class
    {
        public void Add(TModel model);
        public void Update(TModel model);
        public void Delete(int id);
        public void GetById(int id);
        public Task GetByIdAsync(int id);
        public void Save();
        public Task SaveAsync();
    }
}
