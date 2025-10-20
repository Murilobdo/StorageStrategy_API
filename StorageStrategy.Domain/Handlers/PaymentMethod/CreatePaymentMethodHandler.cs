using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.PaymentMethod;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.PaymentMethod;

public class CreatePaymentMethodHandler : PaymentMethodHandleBase, IRequestHandler<CreatePaymentMethodCommand, Result>
{
    public CreatePaymentMethodHandler(IPaymentMethodRepository repo, IMapper mapper) : base(repo, mapper)
    {
    }

    public async Task<Result> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");
        
        var entity = _mapper.Map<PaymentMethodEntity>(request);
        await _repo.AddAsync(entity);
        await _repo.SaveAsync();
        return CreateResponse(entity, "Método de pagamento criado !");
    }
}