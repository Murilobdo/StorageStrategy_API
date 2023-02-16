using AutoMapper;
using StorageStrategy.Domain.Commands.Expenses;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class ExpensesProfile : Profile
    {
        public ExpensesProfile()
        {
            CreateMap<CreateExpensesCommand, ExpensesEntity>()
                .ForMember(p => p.Company, op => op.Ignore())
                .ForMember(p => p.ExpensesType, op => op.Ignore());

            CreateMap<ExpensesEntity, ExpensesCommandBase>();
        }
    }
}
