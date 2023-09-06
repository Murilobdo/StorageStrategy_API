using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using StorageStrategy.Utils.Helpers;

namespace StorageStrategy.Domain.Handlers
{
    public class DashboardHandler : HandlerBase,
        IRequestHandler<EntryAndExitOfMonthCommand, Result>,
        IRequestHandler<EntryAndExitForDayCommand, Result>,
        IRequestHandler<InfoPaymentCommand, Result>,
        IRequestHandler<TotalCostPricePerDayCommand, Result>,
        IRequestHandler<SalesPerEmployeeCommand, Result>
    {
        private readonly ICommandRepository _repoCommand;
        private readonly IEmployeeRepository _repoEmployee;
        private readonly IMapper _mapper;

        public DashboardHandler(ICommandRepository repoCommand, IMapper mapper, IEmployeeRepository repoEmployee)
        {
            _repoCommand = repoCommand;
            _mapper = mapper;
            _repoEmployee = repoEmployee;
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
            var initialDate = new DateTime(DateTime.Now.Year, request.Month, 1);
            var result = new List<EntryAndExitForDayCommand>();

            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados inválidos");

            var commands = await _repoCommand.ReadCommandsForPeriodAsync(request.CompanyId, request.Month);

            do
            {
                var commandItens = await _repoCommand.ReadCommandsForDaysAsync(request.CompanyId, initialDate.Day);

                result.Add(new EntryAndExitForDayCommand
                {
                    CompanyId = request.CompanyId,
                    DayOfMonth = initialDate.Day,
                    Month = request.Month,
                    MoneyOut = commandItens.Sum(p => p.Cost),
                    MoneyIn = commandItens.Sum(p => p.Price)
                });

                initialDate = initialDate.AddDays(1);
            } while (initialDate.Month == request.Month);

            return CreateResponse(result.OrderBy(p => p.DayOfMonth), "Busca realizada");
        }

        public async Task<Result> Handle(InfoPaymentCommand request, CancellationToken cancellationToken)
        {
            var result = new List<InfoPaymentCommand>();
            var initialDate = new DateTime(DateTime.Now.Year, request.Month, 1);

            if(!request.IsValid())
                return CreateError(request.GetErros(), "Dados Inválidos");

            var commands = await _repoCommand.ReadCommandsForPeriodAsync(request.CompanyId, request.Month);

            request.TotalPix = Calc.TotalPriceForPayment(commands, PaymentEnum.Pix);
            request.TotalDebit = Calc.TotalPriceForPayment(commands, PaymentEnum.Debit);
            request.TotalCredit = Calc.TotalPriceForPayment(commands, PaymentEnum.Credit);
            request.TotalCash = Calc.TotalPriceForPayment(commands, PaymentEnum.Cash);
            request.Total = commands.Sum(p => p.TotalPrice);

            return CreateResponse(request, "Busca realizada");

        }

        public async Task<Result> Handle(SalesPerEmployeeCommand request, CancellationToken cancellationToken)
        {
            List<SalesPerEmployee> response = new();

            if(!request.IsValid())
                return CreateError(request.GetErros(), "Dados Inválidos");

            List<CommandEntity> commands = await _repoCommand.ReadCommandsForPeriodAsync(request.CompanyId, request.Month);

            List<EmployeeEntity> employees = await _repoEmployee.ToList(request.CompanyId);

            foreach (var employee in employees)
            {
                var totalSales = commands
                    .Where(p => p.EmployeeId == employee.EmployeeId)
                    .Sum(p => p.TotalPrice);

                response.Add(new SalesPerEmployee(employee.Name, totalSales));
            }

            return CreateResponse(response, "Busca realizada");
        }

        public async Task<Result> Handle(TotalCostPricePerDayCommand request, CancellationToken cancellationToken)
        {
            List<TotalCostPriceResponse> response = new();

            if(!request.IsValid())
                return CreateError(request.GetErros(), "Dados inválidos");

            var commands = await _repoCommand.ReadCommandsForPeriodAsync(request.CompanyId, request.Month);

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
}
