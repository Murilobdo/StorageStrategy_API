
using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Handlers.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

public class EntryAndExitForDayHandle :  DashboardHandlerBase, IRequestHandler<EntryAndExitForDayCommand, Result>
{
    public EntryAndExitForDayHandle(ICommandRepository repoCommand) : base(repoCommand)
    {
    }

    public async Task<Result> Handle(EntryAndExitForDayCommand request, CancellationToken cancellationToken)
    {
        var result = new List<EntryAndExitForDayCommand>();

        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        var commands = await _repoCommand.ReadCommandsForPeriodWithItensAsync(request.CompanyId, request.Month, request.Year);
            
        for (int day = 1; day <= 31; day++)
        {
            var currentCommand = commands
                .Where(p => p.FinalDate.Value.Day == day);
                
            if (!currentCommand.Any())
            {
                result.Add(new EntryAndExitForDayCommand
                {
                    CompanyId = request.CompanyId,
                    DayOfMonth = day,
                    Month = request.Month,
                    MoneyOut = 0,
                    MoneyIn = 0
                });
            }
            else
            {
                result.Add(new EntryAndExitForDayCommand
                {
                    CompanyId = request.CompanyId,
                    DayOfMonth = day,
                    Month = request.Month,
                    MoneyOut = currentCommand
                        .Where(p => p.FinalDate.Value.Day == day)
                        .Sum(p => p.TotalCost),
                    MoneyIn = currentCommand
                        .Where(p => p.FinalDate.Value.Day == day)
                        .Sum(p => p.TotalPrice - p.Discount + p.Increase)
                });
            }
        }

        return CreateResponse(result.OrderBy(p => p.DayOfMonth), "Busca realizada");
    }
}