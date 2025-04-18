using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository;

public class LogRepository : RepositoryBase<LogApp>, ILogRepository
{
    public LogRepository(StorageDbContext context) : base(context)
    {
    }

    public override async Task<LogApp> GetById(int logId)
    {
        return await _context.Logs
            .FirstOrDefaultAsync(x => x.LogId == logId);
    }
}