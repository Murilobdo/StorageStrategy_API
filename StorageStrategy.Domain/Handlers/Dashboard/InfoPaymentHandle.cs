
using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Handlers.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Helpers;

public class InfoPaymentHandle :  DashboardHandlerBase, IRequestHandler<InfoPaymentCommand, Result>
{
    public InfoPaymentHandle(ICommandRepository repoCommand) : base(repoCommand)
    {
    }

    public async Task<Result> Handle(InfoPaymentCommand request, CancellationToken cancellationToken)
    {
        if(!request.IsValid())
            return CreateError(request.GetErros(), "Dados Inválidos");

        var commands = await _repoCommand.ReadMonthCommandsAsync(request.CompanyId, request.Month, request.Year);

        request.TotalPix = Calc.CountSalesPayment(commands, PaymentEnum.Pix);
        request.TotalDebit = Calc.CountSalesPayment(commands, PaymentEnum.Debit);
        request.TotalCredit = Calc.CountSalesPayment(commands, PaymentEnum.Credit);
        request.TotalCash = Calc.CountSalesPayment(commands, PaymentEnum.Cash);
        request.Total = commands.SelectMany(p => p.Payments).Count();

        return CreateResponse(request, "Busca realizada");
    }
}