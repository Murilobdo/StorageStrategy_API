using AutoMapper;
using StorageStrategy.Domain.Repository;

namespace StorageStrategy.Domain.Handlers.PaymentMethod;

public abstract class PaymentMethodHandleBase : HandlerBase
{
    protected IPaymentMethodRepository _repo;
    protected IMapper _mapper;
    
    public PaymentMethodHandleBase(IPaymentMethodRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
}