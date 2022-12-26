using AutoMapper;
using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeCommand, EmployeeEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore());

            CreateMap<EmployeeEntity, CreateEmployeeCommand>();

            CreateMap<UpdateEmployeeCommand, EmployeeEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore());

            CreateMap<DeleteEmployeeCommand, EmployeeEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore());
        }
    }
}
