using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Dashboard;

public class EntryAndExitOfMonthHandle : DashboardHandlerBase, IRequestHandler<EntryAndExitOfMonthCommand, Result>
{
    public EntryAndExitOfMonthHandle(ICommandRepository repoCommand) : base(repoCommand)
    {
    }

    public async Task<Result> Handle(EntryAndExitOfMonthCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos"); 

        var command = await _repoCommand.ReadMonthCommandsAsync(request.CompanyId, request.CurrentDate.Month, request.CurrentDate.Year);

        if (command is null)
            return CreateError("Comanda não encontrada");

        request.MoneyOut = command.Sum(p => p.TotalCost);
        request.MoneyIn = command.Sum(p => p.GetFinalPrice());

        return CreateResponse(request, "Busca realizada");
    }
}