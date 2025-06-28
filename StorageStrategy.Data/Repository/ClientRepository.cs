using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository;

public class ClientRepository : RepositoryBase<ClientEntity>, IClientRepository
{
    public ClientRepository(StorageDbContext context) : base(context)
    {
    }

    public override async Task<ClientEntity> GetById(int id)
    {
        return await _context.Client.FirstOrDefaultAsync(p => p.ClientId == id);
    }

    public async Task<List<ClientEntity>> GetClientsAsync(int companyId)
    {
        return await _context.Client
            .AsNoTracking()
            .Where(p => p.CompanyId == companyId).ToListAsync();
    }

    public async Task<ClientEntity?> GetByNameAsync(int companyId, string requestName)
    {
        return await _context.Client.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Name == requestName);
    }

  
}