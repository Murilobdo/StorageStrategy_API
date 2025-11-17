using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Tests.FakeRepository;

public class FakeCompanyRepository : ICompanyRepository
{
    public Task AddAsync(CompanyEntity model)
    {
        throw new NotImplementedException();
    }

    public Task<List<CompanyEntity>> GetAllAsync(Func<CompanyEntity, bool> filter, string[] includeProperties = null)
    {
        throw new NotImplementedException();
    }

    public void Update(CompanyEntity model)
    {
        throw new NotImplementedException();
    }

    public void Delete(CompanyEntity id)
    {
        throw new NotImplementedException();
    }

    public void RemoveRange(CompanyEntity model)
    {
        throw new NotImplementedException();
    }

    public Task<CompanyEntity> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }

    public Task CreateTranscationAsync()
    {
        throw new NotImplementedException();
    }

    public Task RollbackAsync()
    {
        throw new NotImplementedException();
    }

    public Task CommitAsync()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public Task CreateCompanyExpenseCategorys(List<ExpensesTypeEntity> expensesTypeEntities)
    {
        throw new NotImplementedException();
    }

    public Task<List<CompanyEntity>> ToList()
    {
        throw new NotImplementedException();
    }
}