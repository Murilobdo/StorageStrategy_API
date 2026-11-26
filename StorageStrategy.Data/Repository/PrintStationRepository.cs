using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository;

public class PrintStationRepository : RepositoryBase<PrintStationEntity>, IPrintStationRepository
{
    public PrintStationRepository(StorageDbContext context) : base(context)
    {
    }

    public override async Task<PrintStationEntity> GetById(int id)
    {
        return await _context.PrintStations.FirstOrDefaultAsync(p => p.PrintStationId == id);
    }

    public async Task<PrintStationEntity?> GetActiveByCompanyIdAsync(int companyId)
    {
        return await _context.PrintStations
            .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.IsActive);
    }

    public async Task<PrintStationEntity?> GetActiveByIdAsync(int printStationId)
    {
        return await _context.PrintStations
            .FirstOrDefaultAsync(p => p.PrintStationId == printStationId && p.IsActive);
    }
}
