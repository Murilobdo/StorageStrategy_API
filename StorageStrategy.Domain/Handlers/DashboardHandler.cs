using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

namespace StorageStrategy.Domain.Handlers
{
    public class DashboardHandler : HandlerBase,
        IRequestHandler<EntryAndExitOfMonthCommand, Result>,
        IRequestHandler<EntryAndExitForDayCommand, Result>
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
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados inválidos"); 

            var command = await _repoCommand.ReadCommandsForPeriodAsync(request.CompanyId, request.CurrentDate.Month);

            if (command is null)
                return CreateError("Comanda não encontrada");

            request.MoneyOut = command.Sum(p => p.TotalCost);
            request.MoneyIn = command.Sum(p => p.TotalPrice);

            return CreateResponse(request, "Busca realizada");
        }

        public async Task<Result> Handle(EntryAndExitForDayCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados inválidos");

            var command = await _repoCommand.ReadCommandsForPeriodAsync(request.CompanyId, request.Date.Month);

            var result = new List<EntryAndExitForDayCommand>();
            var initialDate = new DateTime(request.Date.Year, request.Date.Month, 1);

            do
            {
                var commandItens = await _repoCommand.ReadCommandsForDaysAsync(request.CompanyId, initialDate.Day);

                result.Add(new EntryAndExitForDayCommand
                {
                    CompanyId = request.CompanyId,
                    Date = request.Date,
                    DayOfMonth = initialDate.Day,
                    MoneyOut = commandItens.Sum(p => p.Cost),
                    MoneyIn = commandItens.Sum(p => p.Price)
                });

                initialDate = initialDate.AddDays(1);
            } while (initialDate.Month == request.Date.Month);

            return CreateResponse(result.OrderBy(p => p.DayOfMonth), "Busca realizada");
        }
    }
}
