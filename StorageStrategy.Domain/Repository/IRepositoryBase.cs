﻿using System.Linq.Expressions;

namespace StorageStrategy.Domain.Repository
{
    public interface IRepositoryBase<TModel> where TModel : class
    {
        Task AddAsync(TModel model);
        void Update(TModel model);
        void Delete(TModel id);
        void RemoveRange(TModel model);
        Task<TModel> GetById(int id);
        void Save();
        Task SaveAsync();
        Task CreateTranscationAsync();
        Task RollbackAsync();
        Task CommitAsync();
        void Clear();

    }
}
