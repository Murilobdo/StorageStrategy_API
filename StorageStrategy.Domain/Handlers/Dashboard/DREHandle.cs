using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Handlers.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Helpers;

public class DREHandlee : DashboardHandlerBase, IRequestHandler<DRECommand, Result>
{
    public DREHandlee(ICommandRepository repoCommand, IExpenseRepository repoExpenses) : base(repoCommand, repoExpenses)
    {
    }

    public async Task<Result> Handle(DRECommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateResponse(request.GetErros(), "Dados Inválidos !");

        var commands = await _repoCommand.ReadCommandsForPeriodWithItensAsync(request.CompanyId, request.Month, request.Year);
        var expenses = await _repoExpenses.ReadTotalExpensesByMonth(request.CompanyId, request.Month, request.Year);

        request.ReceitaBruta = Calc.GetTotalPriceWithDiscount(commands);
        request.DeducoesAbatimentos = Calc.TotalDeImpostos(commands);
        request.ReceitaLiquida = request.ReceitaBruta - request.DeducoesAbatimentos;
        request.CPV = Calc.CustoProdutoVendido(commands);
        request.LucroBruto = request.ReceitaLiquida - request.CPV;
        request.DespesasAdministrativas = expenses.Sum(p => p.ExpenseValue);
        request.ResultadoLiquido = request.LucroBruto - request.DespesasAdministrativas;

        return CreateResponse(request, "Busca realizada !");    }
}