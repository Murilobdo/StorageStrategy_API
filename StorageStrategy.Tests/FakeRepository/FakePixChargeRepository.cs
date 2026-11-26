using Microsoft.EntityFrameworkCore.Storage;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Tests.FakeRepository
{
    public class FakePixChargeRepository : IPixChargeRepository
    {
        public readonly List<PixChargeEntity> charges;

        public FakePixChargeRepository()
        {
            charges = new();
        }

        public Task AddAsync(PixChargeEntity model)
        {
            if (model.PixChargeId == 0)
                model.PixChargeId = charges.Count + 1;

            charges.Add(model);
            return Task.CompletedTask;
        }

        public Task<List<PixChargeEntity>> GetAllAsync(Func<PixChargeEntity, bool> filter, string[] includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Update(PixChargeEntity model)
        {
        }

        public void Delete(PixChargeEntity model)
        {
            charges.Remove(model);
        }

        public void RemoveRange(PixChargeEntity model)
        {
        }

        public Task<PixChargeEntity> GetById(int id)
        {
            return Task.FromResult(charges.FirstOrDefault(p => p.PixChargeId == id));
        }

        public void Save()
        {
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }

        public Task<IDbContextTransaction> CreateTranscationAsync(CancellationToken cancellationToken)
        {
            return null;
        }

        public Task RollbackAsync()
        {
            return Task.CompletedTask;
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public void Clear()
        {
        }

        public Task<PixChargeEntity?> GetByIdForCompanyAsync(int pixChargeId, int companyId)
        {
            return Task.FromResult(charges.FirstOrDefault(p => p.PixChargeId == pixChargeId && p.CompanyId == companyId));
        }

        public Task<PixChargeEntity?> GetPendingByCommandIdAsync(int commandId, int companyId)
        {
            return Task.FromResult(charges.FirstOrDefault(p =>
                p.CommandId == commandId &&
                p.CompanyId == companyId &&
                (p.Status == PixChargeStatusEnum.Criando || p.Status == PixChargeStatusEnum.AguardandoPagamento) &&
                p.ExpiraEm > DateTime.Now));
        }

        public Task<PixChargeEntity?> GetByMpPaymentIdAsync(long mpPaymentId)
        {
            return Task.FromResult(charges.FirstOrDefault(p => p.MpPaymentId == mpPaymentId));
        }

        public Task<List<PixChargeEntity>> GetExpiredPendingAsync()
        {
            var result = charges
                .Where(p => p.Status == PixChargeStatusEnum.AguardandoPagamento)
                .Where(p => p.ExpiraEm < DateTime.Now)
                .ToList();

            return Task.FromResult(result);
        }
    }
}
