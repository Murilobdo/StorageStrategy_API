using FluentValidation;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Validations.Category;
using StorageStrategy.Domain.Validations.Command;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command
{
    public record class CreateCommandCommand : CommandCommandBase, IValidation
    {
        public CreateCommandCommand(
            int companyId, 
            string name, 
            int employeeId, 
            decimal discount,
            decimal increase,
            List<CommandItemBase> items, 
            PaymentEnum? payment
        ) {
            CompanyId = companyId;
            Name = name;
            EmployeeId = employeeId;
            Discount = discount;
            Increase = increase;
            Items = items;
            Payment = payment;
        }

        public List<Error> GetErros() => new CreateCommandValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new CreateCommandValidation().Validate(this).IsValid;
    }
}
