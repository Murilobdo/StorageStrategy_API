using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository;

public interface IPixCredentialRepository : IRepositoryBase<PixCredentialEntity>
{
    Task<PixCredentialEntity?> GetByCompanyIdAsync(int companyId);
    Task<PixCredentialEntity?> GetActiveByCompanyIdAsync(int companyId);
}
