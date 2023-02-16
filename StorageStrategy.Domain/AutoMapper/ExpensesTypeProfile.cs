using AutoMapper;
using StorageStrategy.Domain.Commands.Expenses;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class ExpensesTypeProfile : Profile
    {
        public ExpensesTypeProfile()
        {
            CreateMap<CreateExpensesTypeCommand, ExpensesTypeEntity>()
                .ForMember(p => p.Company, op => op.Ignore());

            CreateMap<ExpensesTypeEntity, ExpensesTypeCommandBase>();
        }
    }
}
