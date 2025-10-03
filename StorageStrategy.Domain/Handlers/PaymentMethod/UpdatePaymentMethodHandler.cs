using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.PaymentMethod;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.PaymentMethod;

public class UpdatePaymentMethodHandler : PaymentMethodHandleBase, IRequestHandler<UpdatePaymentMethodCommand, Result>
{
    public UpdatePaymentMethodHandler(IPaymentMethodRepository repo, IMapper mapper) : base(repo, mapper)
    {
    }

    public async Task<Result> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");
        
        var entity = await _repo.GetById(request.PaymentMethodId);
        entity = _mapper.Map<PaymentMethodEntity>(request);
        _repo.Update(entity);
        await _repo.SaveAsync();
        
        return CreateResponse(entity, "Método de pagamento criado !");
    }
}