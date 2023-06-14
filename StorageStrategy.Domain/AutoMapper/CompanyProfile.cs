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

            CreateMap<CreateCompanyCommand, CompanyEntity>();
        }
    }
}
