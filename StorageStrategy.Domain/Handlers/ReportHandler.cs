using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Report;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Models.ViewModels.Report;
using StorageStrategy.Utils.Helpers;

namespace StorageStrategy.Domain.Handlers
{
    public class ReportHandler : HandlerBase,
        IRequestHandler<ReadCommandsBetweenDatesCommand, Result>,
        IRequestHandler<ReadCommandsByMounthCommand, Result>,
        IRequestHandler<ReadPaymentCommandCommand, Result>
    {
        
        private IReportRepository _repo;
        public ReportHandler(IReportRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result> Handle(ReadCommandsBetweenDatesCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");
            
            request.FinalDate = request.FinalDate.AddDays(1);
            var commands = await _repo.ReadFinishCommandsByDateAsync(request);
            
            return CreateResponse(new ReportCommandViewModel {
                Commands = commands,
                TotalCostService = Calc.SumCostService(commands),
                TotalCostProduct = Calc.SumCostProduct(commands),
                TotalPriceService = Calc.SumPriceService(commands),
                TotalPriceProduct = Calc.GetTotalPriceWithDiscount(commands),
            }, "Busca realizada !");
        }

        public async Task<Result> Handle(ReadCommandsByMounthCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var commands = await _repo.ReadCommandsByMounthAsync(request.CompanyId, request.Month, request.EmployeeId);

            return CreateResponse(new
            {
                Commands = commands,
                TotalCost = commands.Sum(p => p.TotalCost),
                TotalPrice = commands.Sum(p => p.TotalPrice),
            }, "Busca realizada !");
        }

        public async Task<Result> Handle(ReadPaymentCommandCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var commands = await _repo.ReadCommandsByMounthAsync(request.CompanyId, request.Month, request.EmployeeId);

            decimal totalPix = Calc.CountSalesPayment(commands, PaymentEnum.Pix);
            decimal totalCash = Calc.CountSalesPayment(commands, PaymentEnum.Cash);
            decimal totalCredit = Calc.CountSalesPayment(commands, PaymentEnum.Credit);
            decimal totalDebit = Calc.CountSalesPayment(commands, PaymentEnum.Debit);
            decimal total = (totalPix + totalCash + totalCredit + totalDebit);

            return CreateResponse(new
            {
                PixPercent = new { 
                    Value = totalPix,
                    Percent = Math.Round(totalPix / total, 2)
                },
                CashPercent = new
                {
                    Value = totalPix,
                    Percent = Math.Round(totalCash / total, 2)
                },
                CreditPercent = new
                {
                    Value = totalPix,
                    Percent = Math.Round(totalCredit / total, 2)
                },
                DebitPercent = new
                {
                    Value = totalPix,
                    Percent = Math.Round(totalDebit / total, 2)
                },
            }, "Busca realizada !");
        }
    }
}