using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Helpers;

namespace StorageStrategy.Domain.Handlers.Dashboard;

public class TotalSalesPerCategoryProductHandle : DashboardHandlerBase, IRequestHandler<TotalSalesPerCategoryProductCommand, Result>
{
    public TotalSalesPerCategoryProductHandle(ICommandRepository repoCommand) : base(repoCommand)
    {
    }

    public async Task<Result> Handle(TotalSalesPerCategoryProductCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        var commands = await _repoCommand.ReadCommandsForPeriodWithItensAsync(request.CompanyId, request.Month, request.Year);

        request.TotalSalesPerCategory = Calc.TotalSalesPerCategory(commands);

        return CreateResponse(request.TotalSalesPerCategory, "Busca realizada !");
    }
}
