using AutoMapper;
using StorageStrategy.Domain.Commands.Expenses;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class ExpensesProfile : Profile
    {
        public ExpensesProfile()
        {
            CreateMap<CreateExpenseCommand, ExpenseEntity>()
                .ForMember(p => p.Company, op => op.Ignore())
                .ForMember(p => p.ExpensesType, op => op.Ignore());

            CreateMap<ExpenseEntity, ExpenseCommandBase>();
        }
    }
}
