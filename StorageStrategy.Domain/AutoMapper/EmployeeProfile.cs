using AutoMapper;
using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeEntity, CreateEmployeeCommand>();

            CreateMap<CreateEmployeeCommand, EmployeeEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.PasswordHash, opt => opt.Ignore());

            CreateMap<UpdateEmployeeCommand, EmployeeEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.PasswordHash, opt => opt.Ignore());

            CreateMap<DeleteEmployeeCommand, EmployeeEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.PasswordHash, opt => opt.Ignore());

        }
    }
}
