using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Report;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class ReportHandler : HandlerBase,
        IRequestHandler<ReadCommandsBetweenDatesCommand, Result>,
        IRequestHandler<ReadCommandsByMounthCommand, Result>,
        IRequestHandler<ReadPaymentCommandCommand, Result>
    {
        
        private IReportRepository _repo;
        private ICategoryRepository _repoCategory;
        private IMapper _mapper;

        public ReportHandler(IReportRepository repo, ICategoryRepository repoCategory, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _repoCategory = repoCategory;
        }

        public async Task<Result> Handle(ReadCommandsBetweenDatesCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var commands = await _repo.ReadCommandsByDateAsync(request.CompanyId, request.InitialDate, request.FinalDate.AddDays(1), request.EmployeeId);

            return CreateResponse(new {
                Commands = commands,
                TotalCost = commands.Sum(p => p.TotalCost),
                TotalPrice = commands.Sum(p => p.TotalPrice),
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

            decimal totalPix = commands.Where(p => p.Payment == PaymentEnum.Pix).Sum(p => p.TotalPrice);
            decimal totalCash = commands.Where(p => p.Payment == PaymentEnum.Cash).Sum(p => p.TotalPrice);
            decimal totalCredit = commands.Where(p => p.Payment == PaymentEnum.Credit).Sum(p => p.TotalPrice);
            decimal totalDebit = commands.Where(p => p.Payment == PaymentEnum.Debit).Sum(p => p.TotalPrice);
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