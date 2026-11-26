using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository;

public interface IPrintStationRepository : IRepositoryBase<PrintStationEntity>
{
    Task<PrintStationEntity?> GetActiveByCompanyIdAsync(int companyId);

    /// <summary>Usado na autenticação do agente: a empresa vem da estação, nunca do cliente.</summary>
    Task<PrintStationEntity?> GetActiveByIdAsync(int printStationId);
}
