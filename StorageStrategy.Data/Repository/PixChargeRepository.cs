using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository;

public class PixChargeRepository : RepositoryBase<PixChargeEntity>, IPixChargeRepository
{
    public PixChargeRepository(StorageDbContext context) : base(context)
    {
    }

    public override async Task<PixChargeEntity> GetById(int id)
    {
        return await _context.PixCharges.FirstOrDefaultAsync(p => p.PixChargeId == id);
    }

    public async Task<PixChargeEntity?> GetByIdForCompanyAsync(int pixChargeId, int companyId)
    {
        return await _context.PixCharges
            .FirstOrDefaultAsync(p => p.PixChargeId == pixChargeId && p.CompanyId == companyId);
    }

    public async Task<PixChargeEntity?> GetPendingByCommandIdAsync(int commandId, int companyId)
    {
        return await _context.PixCharges
            .Where(p => p.CommandId == commandId && p.CompanyId == companyId)
            .Where(p => p.Status == PixChargeStatusEnum.Criando || p.Status == PixChargeStatusEnum.AguardandoPagamento)
            .Where(p => p.ExpiraEm > DateTime.Now)
            .FirstOrDefaultAsync();
    }

    public async Task<PixChargeEntity?> GetByMpPaymentIdAsync(long mpPaymentId)
    {
        return await _context.PixCharges.FirstOrDefaultAsync(p => p.MpPaymentId == mpPaymentId);
    }

    public async Task<List<PixChargeEntity>> GetExpiredPendingAsync()
    {
        return await _context.PixCharges
            .Where(p => p.Status == PixChargeStatusEnum.AguardandoPagamento)
            .Where(p => p.ExpiraEm < DateTime.Now)
            .ToListAsync();
    }
}
