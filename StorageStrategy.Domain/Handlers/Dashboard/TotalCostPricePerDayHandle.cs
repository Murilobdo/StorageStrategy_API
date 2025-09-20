
using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Handlers.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

public class TotalCostPricePerDay : DashboardHandlerBase, IRequestHandler<TotalCostPricePerDayCommand, Result>
{
    public TotalCostPricePerDay(ICommandRepository repoCommand) : base(repoCommand)
    {
    }

    public async Task<Result> Handle(TotalCostPricePerDayCommand request, CancellationToken cancellationToken)
    {
        List<TotalCostPriceResponse> response = new();

        if(!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        var commands = await _repoCommand.ReadMonthCommandsAsync(request.CompanyId, request.Month, request.Year);

        foreach (var command in commands.GroupBy(p => p.InitialDate.Day))
        {
            response.Add(new TotalCostPriceResponse(
                day: command.First().InitialDate.Day,
                totalPrice: command.Sum(p => p.TotalPrice),
                totalCost: command.Sum(p => p.TotalCost)
            ));
        }

        return CreateResponse(response.OrderBy(p => p.TotalPrice), "Busca realizada");
    }
}