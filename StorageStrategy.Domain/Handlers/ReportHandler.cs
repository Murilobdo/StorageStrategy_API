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
            if(!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");
            
            CommandsByMounthViewModel response = new();
            response.Commands = await _repo.ReadCommandsByDateAsync(
                request.CompanyId, 
                request.InitialDate,
                request.FinalDate,
                request.EmployeeId);
            
            foreach (var item in response.Commands.SelectMany(p => p.Items))
            {
                response.TotalCost += item.Cost;
                response.TotalPrice += item.Price;
            }
            
            return CreateResponse(response, "Busca realizada");
        }
    }
}