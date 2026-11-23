using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository;

public class PixCredentialRepository : RepositoryBase<PixCredentialEntity>, IPixCredentialRepository
{
    public PixCredentialRepository(StorageDbContext context) : base(context)
    {
    }

    public override async Task<PixCredentialEntity> GetById(int id)
    {
        return await _context.PixCredentials.FirstOrDefaultAsync(p => p.PixCredentialId == id);
    }

    public async Task<PixCredentialEntity?> GetByCompanyIdAsync(int companyId)
    {
        return await _context.PixCredentials.FirstOrDefaultAsync(p => p.CompanyId == companyId);
    }

    public async Task<PixCredentialEntity?> GetActiveByCompanyIdAsync(int companyId)
    {
        return await _context.PixCredentials.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.IsActive);
    }
}
