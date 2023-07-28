using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Report;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.ViewModels;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class ReportHandler : HandlerBase,
        IRequestHandler<ReadCommandsByMounthCommand, Result>
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

        public async Task<Result> Handle(ReadCommandsByMounthCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var commands = await _repo.ReadCommandsByDateAsync(request.CompanyId, request.InitialDate, request.FinalDate, request.EmployeeId);

            return CreateResponse(new {
                Commands = commands,
                TotalCost = commands.Sum(p => p.TotalCost),
                TotalPrice = commands.Sum(p => p.TotalPrice),
            }, "Busca realizada !");
        }
    }
}