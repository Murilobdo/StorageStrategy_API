using Microsoft.EntityFrameworkCore.Storage;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Tests.FakeRepository
{
    /// <summary>Fila em memória com a mesma semântica do PrintJobRepository (claim atômico por empresa).</summary>
    public class FakePrintJobRepository : IPrintJobRepository
    {
        public readonly List<PrintJobEntity> jobs = new();
        public readonly List<CommandEntity> commands = new();

        public Task AddAsync(PrintJobEntity model)
        {
            if (model.PrintJobId == 0)
                model.PrintJobId = jobs.Count == 0 ? 1 : jobs.Max(p => p.PrintJobId) + 1;

            jobs.Add(model);
            return Task.CompletedTask;
        }

        public Task<CommandEntity?> GetCommandForPrintAsync(int commandId, int companyId) =>
            Task.FromResult(commands.FirstOrDefault(p => p.CommandId == commandId && p.CompanyId == companyId));

        public Task<PrintJobEntity?> GetByIdForCompanyAsync(int printJobId, int companyId) =>
            Task.FromResult(jobs.FirstOrDefault(p => p.PrintJobId == printJobId && p.CompanyId == companyId));

        public Task<PrintJobEntity?> GetLatestByCommandAsync(int commandId, int companyId) =>
            Task.FromResult(jobs
                .Where(p => p.CommandId == commandId && p.CompanyId == companyId)
                .OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.PrintJobId)
                .FirstOrDefault());

        public Task<PrintJobEntity?> GetRecentDuplicateAsync(int commandId, int employeeId, int companyId, DateTime since) =>
            Task.FromResult(jobs
                .Where(p => p.CommandId == commandId && p.CompanyId == companyId)
                .Where(p => p.RequestedByEmployeeId == employeeId && p.CreatedAt >= since)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefault());

        public Task<int> CountPrintedAsync(int commandId, int companyId) =>
            Task.FromResult(jobs.Count(p => p.CommandId == commandId
                                            && p.CompanyId == companyId
                                            && p.Status == PrintJobStatusEnum.Impresso));

        public Task<PrintJobEntity?> ClaimNextAsync(int companyId, int printStationId, Guid claimId, DateTime now)
        {
            lock (jobs)
            {
                var next = jobs
                    .Where(p => p.CompanyId == companyId && p.Status == PrintJobStatusEnum.Aguardando)
                    .OrderBy(p => p.CreatedAt)
                    .ThenBy(p => p.PrintJobId)
                    .FirstOrDefault();

                if (next is null)
                    return Task.FromResult<PrintJobEntity?>(null);

                next.Status = PrintJobStatusEnum.Imprimindo;
                next.ClaimedAt = now;
                next.ClaimId = claimId;
                next.PrintStationId = printStationId;
                return Task.FromResult<PrintJobEntity?>(next);
            }
        }

        public Task<List<PrintJobEntity>> GetWaitingCreatedBeforeAsync(DateTime limit) =>
            Task.FromResult(jobs.Where(p => p.Status == PrintJobStatusEnum.Aguardando && p.CreatedAt < limit).ToList());

        public Task<List<PrintJobEntity>> GetClaimedBeforeAsync(DateTime limit) =>
            Task.FromResult(jobs.Where(p => p.Status == PrintJobStatusEnum.Imprimindo && p.ClaimedAt < limit).ToList());

        public Task<int> DeleteCreatedBeforeAsync(DateTime limit) =>
            Task.FromResult(jobs.RemoveAll(p => p.CreatedAt < limit));

        public Task<PrintJobEntity> GetById(int id) =>
            Task.FromResult(jobs.FirstOrDefault(p => p.PrintJobId == id)!);

        public Task<List<PrintJobEntity>> GetAllAsync(Func<PrintJobEntity, bool> filter, string[] includeProperties = null) =>
            Task.FromResult(jobs.Where(filter).ToList());

        public void Update(PrintJobEntity model) { }
        public void Delete(PrintJobEntity model) => jobs.Remove(model);
        public void RemoveRange(PrintJobEntity model) { }
        public void Save() { }
        public Task SaveAsync() => Task.CompletedTask;
        public Task<IDbContextTransaction> CreateTranscationAsync(CancellationToken cancellationToken) => null!;
        public Task RollbackAsync() => Task.CompletedTask;
        public Task CommitAsync() => Task.CompletedTask;
        public void Clear() { }
    }
}
