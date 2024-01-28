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
        IRequestHandler<SalesPerEmployeeCommand, Result>,
        IRequestHandler<InfoCardCommand, Result>,
        IRequestHandler<TotalSalesPerCategoryProductCommand, Result>,
        IRequestHandler<DRECommand, Result>
    {
        
        private readonly ICommandRepository _repoCommand;
        private readonly IEmployeeRepository _repoEmployee;
        private readonly IMapper _mapper;
        private readonly IProductRepository _repoProduct;
        private readonly IExpenseRepository _repoExpenses;

        public DashboardHandler(
            ICommandRepository repoCommand, 
            IMapper mapper, 
            IEmployeeRepository repoEmployee, 
            IProductRepository repoProduct,
            IExpenseRepository repoExpenses)
        {
            _repoCommand = repoCommand;
            _mapper = mapper;
            _repoEmployee = repoEmployee;
            _repoProduct = repoProduct;
            _repoExpenses = repoExpenses;
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

            var commands = await _repoCommand.ReadCommandsForPeriodAsync(request.CompanyId, initialDate.Month);
            
            for (int day = 0; day < 31; day++)
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

        public async Task<Result> Handle(InfoPaymentCommand request, CancellationToken cancellationToken)
        {
            var result = new List<InfoPaymentCommand>();
            var initialDate = new DateTime(DateTime.Now.Year, request.Month, 1);

            if(!request.IsValid())
                return CreateError(request.GetErros(), "Dados Inválidos");

            var commands = await _repoCommand.ReadCommandsForPeriodAsync(request.CompanyId, request.Month);

            request.TotalPix = Calc.CountSalesPayment(commands, PaymentEnum.Pix);
            request.TotalDebit = Calc.CountSalesPayment(commands, PaymentEnum.Debit);
            request.TotalCredit = Calc.CountSalesPayment(commands, PaymentEnum.Credit);
            request.TotalCash = Calc.CountSalesPayment(commands, PaymentEnum.Cash);
            request.Total = commands.Count;

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

        public async Task<Result> Handle(TotalSalesPerCategoryProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados inválidos");

            var commands = await _repoCommand.ReadCommandsForPeriodWithItensAsync(request.CompanyId, request.Month);

            request.TotalSalesPerCategory = Calc.TotalSalesPerCategory(commands);

            return CreateResponse(request.TotalSalesPerCategory, "Busca realizada !");
        }

        public async Task<Result> Handle(InfoCardCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateResponse(request.GetErros(), "Dados Inválidos !");

            var expensesMonth = await _repoExpenses.ReadTotalExpensesByMonth(request.CompanyId, request.Month);

            request.TotalPriceInStok = await _repoProduct.ReadTotalPriceInStokByCompany(request.CompanyId);
            request.TotalMonthExpenses = expensesMonth.Sum(p => p.ExpenseValue);
            request.TotalSales = await _repoCommand.ReadTotalSalesByCompany(request.CompanyId, request.Month);

            return CreateResponse(request, "Busca realizada !");
        }

        public async Task<Result> Handle(DRECommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateResponse(request.GetErros(), "Dados Inválidos !");

            var commands = await _repoCommand.ReadCommandsForPeriodWithItensAsync(request.CompanyId, request.Month);
            var expenses = await _repoExpenses.ReadTotalExpensesByMonth(request.CompanyId, request.Month);

            request.ReceitaBruta = commands.Sum(p => p.TotalPrice);
            request.DeducoesAbatimentos = Calc.TotalDeImpostos(commands);
            request.ReceitaLiquida = request.ReceitaBruta - request.DeducoesAbatimentos;
            request.CPV = Calc.CustoProdutoVendido(commands);
            request.LucroBruto = request.ReceitaLiquida - request.CPV;


            return CreateResponse(request, "Busca realizada !");
        }
    }
}
