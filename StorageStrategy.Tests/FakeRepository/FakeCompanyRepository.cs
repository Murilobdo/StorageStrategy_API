using Microsoft.EntityFrameworkCore.Storage;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Tests.FakeRepository;

public class FakeCompanyRepository : ICompanyRepository
{
    public List<CompanyEntity> companies = new();

    public Task AddAsync(CompanyEntity model)
    {
        companies.Add(model);
        return Task.CompletedTask;
    }

    public Task<List<CompanyEntity>> GetAllAsync(Func<CompanyEntity, bool> filter, string[] includeProperties = null)
    {
        return Task.FromResult(companies.Where(filter).ToList());
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
        return Task.CompletedTask;
    }

    public Task<IDbContextTransaction> CreateTranscationAsync(CancellationToken cancellationToken)
    {
        return null;
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