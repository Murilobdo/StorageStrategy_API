using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Report;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.ViewModels;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class ReportHandler : HandlerBase
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
    }
}