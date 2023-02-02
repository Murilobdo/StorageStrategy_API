using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class DashboardHandler : HandlerBase,
        IRequestHandler<EntryAndExitOfMonthCommand, Result>
    {
        private readonly ICommandRepository _repoCommand;
        private readonly IMapper _mapper;

        public DashboardHandler(ICommandRepository repoCommand, IMapper mapper)
        {
            _repoCommand = repoCommand;
            _mapper = mapper;
        }

        public async Task<Result> Handle(EntryAndExitOfMonthCommand request, CancellationToken cancellationToken)
        {
            var command = await _repoCommand.ReadCommandsForPeriod(request.CompanyId, request.CurrentDate.Month);

            if (command is null)
                return CreateError("Comanda não encontrada");

            request.MoneyOut = command.Sum(p => p.TotalCost);
            request.MoneyIn = command.Sum(p => p.TotalPrice);

            return CreateResponse(request, "Busca realizada");
        }
    }
}
