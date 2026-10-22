using Microsoft.EntityFrameworkCore.Storage;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Tests.FakeRepository
{
    public class FakePixCredentialRepository : IPixCredentialRepository
    {
        public readonly List<PixCredentialEntity> credentials;

        public FakePixCredentialRepository()
        {
            credentials = new()
            {
                new PixCredentialEntity
                {
                    PixCredentialId = 1,
                    CompanyId = 1,
                    AccessTokenEncrypted = "cifrado-access-token",
                    AccessTokenLastDigits = "1234",
                    WebhookSecretEncrypted = "cifrado-webhook-secret",
                    IsActive = true
                }
            };
        }

        public Task AddAsync(PixCredentialEntity model)
        {
            credentials.Add(model);
            return Task.CompletedTask;
        }

        public Task<List<PixCredentialEntity>> GetAllAsync(Func<PixCredentialEntity, bool> filter, string[] includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Update(PixCredentialEntity model)
        {
        }

        public void Delete(PixCredentialEntity model)
        {
            credentials.Remove(model);
        }

        public void RemoveRange(PixCredentialEntity model)
        {
        }

        public Task<PixCredentialEntity> GetById(int id)
        {
            return Task.FromResult(credentials.FirstOrDefault(p => p.PixCredentialId == id));
        }

        public void Save()
        {
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
            return Task.CompletedTask;
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public void Clear()
        {
        }

        public Task<PixCredentialEntity?> GetByCompanyIdAsync(int companyId)
        {
            return Task.FromResult(credentials.FirstOrDefault(p => p.CompanyId == companyId));
        }

        public Task<PixCredentialEntity?> GetActiveByCompanyIdAsync(int companyId)
        {
            return Task.FromResult(credentials.FirstOrDefault(p => p.CompanyId == companyId && p.IsActive));
        }
    }
}
