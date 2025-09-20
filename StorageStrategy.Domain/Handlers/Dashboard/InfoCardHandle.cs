using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Handlers.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Helpers;

public class InfoCard : DashboardHandlerBase, IRequestHandler<InfoCardCommand, Result>
{
    public InfoCard(
        ICommandRepository repoCommand, 
        IExpenseRepository repoExpense, 
        IProductRepository repoProduct
    ) : base(repoCommand, repoExpense, repoProduct)
    {
    }

    public async Task<Result> Handle(InfoCardCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateResponse(request.GetErros(), "Dados Inválidos !");

        List<CommandEntity> commands = await _repoCommand.ReadMonthCommandsAsync(request.CompanyId, request.Month, request.Year);
        
        var expensesMonth = await _repoExpenses.ReadTotalExpensesByMonth(request.CompanyId, request.Month, request.Year);
        request.TotalProfit = Calc.TotalProfitCommands(commands);
        request.TotalMonthExpenses = expensesMonth.Sum(p => p.ExpenseValue);
        request.TotalSales = await _repoCommand.ReadTotalSalesByCompany(request.CompanyId, request.Month);

        return CreateResponse(request, "Busca realizada !");
    }
}