using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository;

public class PaymentMethodRepository : RepositoryBase<PaymentMethodEntity>, IPaymentMethodRepository
{
    public PaymentMethodRepository(StorageDbContext context) : base(context)
    {
    }

    public override async Task<PaymentMethodEntity> GetById(int id)
    {
         return await _context.PaymentMethod.FirstOrDefaultAsync(p => p.PaymentMethodId == id);
    }
}