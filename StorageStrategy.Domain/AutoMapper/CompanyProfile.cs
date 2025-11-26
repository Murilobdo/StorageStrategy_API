using AutoMapper;
using StorageStrategy.Domain.Commands.Company;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CompanyEntity, CompanyCommandBase>();

            CreateMap<CreateCompanyCommand, CompanyEntity>()
                .ForMember(p => p.Categorys, cfg => cfg.Ignore())
                .ForMember(p => p.Commands, cfg => cfg.Ignore())
                .ForMember(p => p.Employees, cfg => cfg.Ignore())
                .ForMember(p => p.Expenses, cfg => cfg.Ignore())
                .ForMember(p => p.StockHistory, cfg => cfg.Ignore())
                .ForMember(p => p.Products, cfg => cfg.Ignore());
        }
    }
}
