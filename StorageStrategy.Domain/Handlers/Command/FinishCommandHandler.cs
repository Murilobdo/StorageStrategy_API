using AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Helpers;

namespace StorageStrategy.Domain.Handlers.Command;

public class FinishCommandHandler : CommandHandlerBase<FinishCommandCommand>
{
    public FinishCommandHandler(
        IProductRepository repoProduct, 
        ICommandRepository repoCommand, 
        IEmployeeRepository repoEmployee, 
        IMapper mapper, 
        IClientRepository clientRepo
    ) : base(repoProduct, repoCommand, repoEmployee, mapper, clientRepo)
    {
    }

    public override async Task<Result> Handle(FinishCommandCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
        if(command is null)
            return CreateError("Comanda não encontrada");
            
        command.AddIncrease(request.Increase);
        command.AddDiscount(request.Discount);
        
        var commandTotal = command.Items.Sum(p => p.Price * p.Qtd) + command.Increase - command.Discount;
        var totalPaid = command.Payments.Sum(p => p.Amount);
        var newPaymentsTotal = request.Payments.Sum(p => p.Amount);
        var totalAfterPayment = totalPaid + newPaymentsTotal;

        if (totalAfterPayment > commandTotal + 0.01m)
            return CreateError($"Total de pagamentos (R$ {totalAfterPayment:F2}) excede o total da comanda (R$ {commandTotal:F2})");


        if (request.Payments.Any(p => p.Amount > 0))
        {
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
        }
        
        string messageResponse = "Comanda atualizada com sucesso";
        if (Math.Abs(commandTotal - totalAfterPayment) == 0)
        {
            command.FinishCommand();
            messageResponse =  "Comanda finalizada com sucesso";
        }
            
        _repoCommand.Update(command);
        await _repoCommand.SaveAsync();

        return CreateResponse(command, messageResponse);
    }
}