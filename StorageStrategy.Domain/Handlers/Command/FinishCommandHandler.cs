using AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Helpers;

namespace StorageStrategy.Domain.Handlers.Command;

public class FinishCommandHandler : CommandHandlerBase<FinishCommandCommand>
{
    public FinishCommandHandler(IProductRepository repoProduct, ICommandRepository repoCommand, IEmployeeRepository repoEmployee, IMapper mapper, IClientRepository clientRepo) : base(repoProduct, repoCommand, repoEmployee, mapper, clientRepo)
    {
    }

    public override async Task<Result> Handle(FinishCommandCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
        if(command is null)
            return CreateError("Comanda não encontrada");
            
        var payments = request.Payments
            .Select(_payment => new PaymentEntity(
                0,
                command.CommandId,
                _payment.Method,
                _payment.Amount,
                _payment.PaymentMethodId,
                _payment.TotalFee
            )).ToList();
            
        command.Payments.AddRange(payments);
        command.AddIncrease(request.Increase);
        command.AddDiscount(request.Discount);

        string messageResponse = "Comanda atualizada com sucesso";
        if (Calc.CommandHasFinishWithTotalPayments(command))
        {
            command.FinishCommand();
            messageResponse =  "Comanda finalizada com sucesso";
        }
            
        _repoCommand.Update(command);
        await _repoCommand.SaveAsync();

        return CreateResponse(command, messageResponse);
    }
}