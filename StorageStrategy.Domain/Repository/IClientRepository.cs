using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository;

public interface IClientRepository : IRepositoryBase<ClientEntity>
{
    Task<List<ClientEntity>> GetClientsAsync(int companyId);
    Task<ClientEntity?> GetByNameAsync(int requestCompanyId, string requestName);
}