using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository;

public class PrintJobRepository : RepositoryBase<PrintJobEntity>, IPrintJobRepository
{
    public PrintJobRepository(StorageDbContext context) : base(context)
    {
    }

    public override async Task<PrintJobEntity> GetById(int id)
    {
        return await _context.PrintJobs.FirstOrDefaultAsync(p => p.PrintJobId == id);
    }

    public async Task<CommandEntity?> GetCommandForPrintAsync(int commandId, int companyId)
    {
        return await _context.Command
            .AsNoTracking()
            .Include(p => p.Items)
            .Include(p => p.Client)
            .Include(p => p.Employee)
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.CommandId == commandId);
    }

    public async Task<PrintJobEntity?> GetByIdForCompanyAsync(int printJobId, int companyId)
    {
        return await _context.PrintJobs
            .FirstOrDefaultAsync(p => p.PrintJobId == printJobId && p.CompanyId == companyId);
    }

    public async Task<PrintJobEntity?> GetLatestByCommandAsync(int commandId, int companyId)
    {
        return await _context.PrintJobs
            .AsNoTracking()
            .Where(p => p.CommandId == commandId && p.CompanyId == companyId)
            .OrderByDescending(p => p.CreatedAt)
            .ThenByDescending(p => p.PrintJobId)
            .FirstOrDefaultAsync();
    }

    public async Task<PrintJobEntity?> GetRecentDuplicateAsync(int commandId, int employeeId, int companyId, DateTime since)
    {
        return await _context.PrintJobs
            .AsNoTracking()
            .Where(p => p.CommandId == commandId && p.CompanyId == companyId)
            .Where(p => p.RequestedByEmployeeId == employeeId && p.CreatedAt >= since)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<int> CountPrintedAsync(int commandId, int companyId)
    {
        return await _context.PrintJobs
            .CountAsync(p => p.CommandId == commandId
                             && p.CompanyId == companyId
                             && p.Status == PrintJobStatusEnum.Impresso);
    }

    public async Task<PrintJobEntity?> ClaimNextAsync(int companyId, int printStationId, Guid claimId, DateTime now)
    {
        // UPDLOCK + READPAST: duas chamadas simultâneas nunca reservam o mesmo job e uma não espera a outra.
        // Um único comando: escolhe e reserva de forma atômica (research R4).
        var waiting = (int)PrintJobStatusEnum.Aguardando;
        var printing = (int)PrintJobStatusEnum.Imprimindo;

        var claimedIds = await _context.Database.SqlQuery<int>($"""
            WITH next AS (
                SELECT TOP (1) *
                FROM PrintJob WITH (ROWLOCK, UPDLOCK, READPAST)
                WHERE CompanyId = {companyId} AND Status = {waiting}
                ORDER BY CreatedAt, PrintJobId
            )
            UPDATE next
            SET Status = {printing}, ClaimedAt = {now}, ClaimId = {claimId}, PrintStationId = {printStationId}
            OUTPUT inserted.PrintJobId AS Value;
            """).ToListAsync();

        if (claimedIds.Count == 0)
            return null;

        return await _context.PrintJobs
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PrintJobId == claimedIds[0]);
    }

    public async Task<List<PrintJobEntity>> GetWaitingCreatedBeforeAsync(DateTime limit)
    {
        return await _context.PrintJobs
            .Where(p => p.Status == PrintJobStatusEnum.Aguardando && p.CreatedAt < limit)
            .ToListAsync();
    }

    public async Task<List<PrintJobEntity>> GetClaimedBeforeAsync(DateTime limit)
    {
        return await _context.PrintJobs
            .Where(p => p.Status == PrintJobStatusEnum.Imprimindo && p.ClaimedAt < limit)
            .ToListAsync();
    }

    public async Task<int> DeleteCreatedBeforeAsync(DateTime limit)
    {
        return await _context.PrintJobs
            .Where(p => p.CreatedAt < limit)
            .ExecuteDeleteAsync();
    }
}
