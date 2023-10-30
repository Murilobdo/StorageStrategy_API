using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using System.ComponentModel.Design;
using System.Xml.Linq;

namespace StorageStrategy.Tests.FakeRepository
{
    public class FakeCategoryRepository : ICategoryRepository
    {

        public readonly List<CategoryEntity> categorys;

        public FakeCategoryRepository()
        {
            categorys = new()
            {
                new CategoryEntity(categoryId: 1, name: "Categoria 1", companyId:1),
                new CategoryEntity(categoryId: 2, name: "Categoria 2", companyId:1),
                new CategoryEntity(categoryId: 3, name: "Categoria 3", companyId:1),
                new CategoryEntity(categoryId: 4, name: "Categoria 4", companyId:1)
            };
        }

        public Task AddAsync(CategoryEntity model)
        {
            return Task.CompletedTask;
        }

        public Task AddRange(IEnumerable<CategoryEntity> categorys)
        {
            return Task.CompletedTask;
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public Task CreateTranscationAsync()
        {
            return Task.CompletedTask;
        }

        public void Delete(CategoryEntity id)
        {

        }

        public Task<CategoryEntity> FindByName(string name, int companyId)
        {
            var category = categorys
                .FirstOrDefault(p => p.Name.ToLower().Trim().Equals(name.ToLower().Trim()) && p.CompanyId == companyId);
            
            return Task.FromResult(category);
        }

        public Task<CategoryEntity> GetById(int id)
        {
            var category = categorys
                .FirstOrDefault(p => p.CategoryId == id);

            return Task.FromResult(category);
        }

        public void RemoveRange(CategoryEntity model)
        {

        }

        public Task RollbackAsync()
        {
            return Task.CompletedTask;
        }

        public void Save()
        {
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }

        public Task<List<CategoryEntity>> ToList(int companyId)
        {
            return Task.FromResult(categorys.Where(p => p.CompanyId == companyId).ToList());
        }

        public void Update(CategoryEntity model)
        {
        }
    }
}
