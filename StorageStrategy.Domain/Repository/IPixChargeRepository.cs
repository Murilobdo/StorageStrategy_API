using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository;

public interface IPixChargeRepository : IRepositoryBase<PixChargeEntity>
{
    Task<PixChargeEntity?> GetByIdForCompanyAsync(int pixChargeId, int companyId);
    Task<PixChargeEntity?> GetPendingByCommandIdAsync(int commandId, int companyId);
    Task<PixChargeEntity?> GetByMpPaymentIdAsync(long mpPaymentId);
    Task<List<PixChargeEntity>> GetExpiredPendingAsync();
}
