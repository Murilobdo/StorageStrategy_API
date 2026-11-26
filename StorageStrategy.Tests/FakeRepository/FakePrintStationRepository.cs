using Microsoft.EntityFrameworkCore.Storage;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Tests.FakeRepository
{
    public class FakePrintStationRepository : IPrintStationRepository
    {
        public readonly List<PrintStationEntity> stations = new();
        public int SaveCount { get; private set; }

        public Task AddAsync(PrintStationEntity model)
        {
            if (model.PrintStationId == 0)
                model.PrintStationId = stations.Count == 0 ? 1 : stations.Max(p => p.PrintStationId) + 1;

            stations.Add(model);
            return Task.CompletedTask;
        }

        public Task<PrintStationEntity?> GetActiveByCompanyIdAsync(int companyId) =>
            Task.FromResult(stations.FirstOrDefault(p => p.CompanyId == companyId && p.IsActive));

        public Task<PrintStationEntity?> GetActiveByIdAsync(int printStationId) =>
            Task.FromResult(stations.FirstOrDefault(p => p.PrintStationId == printStationId && p.IsActive));

        public Task<PrintStationEntity> GetById(int id) =>
            Task.FromResult(stations.FirstOrDefault(p => p.PrintStationId == id)!);

        public Task<List<PrintStationEntity>> GetAllAsync(Func<PrintStationEntity, bool> filter, string[] includeProperties = null) =>
            Task.FromResult(stations.Where(filter).ToList());

        public void Update(PrintStationEntity model) { }
        public void Delete(PrintStationEntity model) => stations.Remove(model);
        public void RemoveRange(PrintStationEntity model) { }
        public void Save() => SaveCount++;

        public Task SaveAsync()
        {
            SaveCount++;
            return Task.CompletedTask;
        }

        public Task<IDbContextTransaction> CreateTranscationAsync(CancellationToken cancellationToken) => null!;
        public Task RollbackAsync() => Task.CompletedTask;
        public Task CommitAsync() => Task.CompletedTask;
        public void Clear() { }
    }
}
