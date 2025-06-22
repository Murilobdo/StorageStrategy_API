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
            PaymentCommand payment
        ) {
            CompanyId = companyId;
            Name = name;
            EmployeeId = employeeId;
            Discount = discount;
            Increase = increase;
            Items = items;
            Payments = new List<PaymentCommand>{payment};
        }

        public CreateCommandCommand()
        {
            
        }

        public CreateCommandCommand(CommandEntity command)
        {
            CommandId = command.CommandId;
            ClientId = command.ClientId.Value;
            Name = command.Name;
            EmployeeId = command.EmployeeId;
            CompanyId = command.CompanyId;
            Discount = command.Discount;
            Increase = command.Increase;
            InitialDate = command.InitialDate;
            FinalDate = command.FinalDate;
            Items = command.Items.Select(p => new CommandItemBase
            {
                CommandItemId = p.CommandItemId,
                ProductId = p.ProductId,
                Qtd = p.Qtd,
                Name = p.Name,
                Price = p.Price
            }).ToList();
            Payments = command.Payments.Select(p => new PaymentCommand
            {
                Method = p.Method,
                Amount = p.Amount
            }).ToList();
            TotalPrice = Items.Sum(p => p.Price * p.Qtd);
        }
        
        public List<Error> GetErros() => new CreateCommandValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new CreateCommandValidation().Validate(this).IsValid;

        public CommandEntity CreateCommand()
        {
            return new CommandEntity
            {
                Name = Name,
                EmployeeId = EmployeeId,
                ClientId = ClientId,
                CompanyId = CompanyId,
                Discount = Discount,
                Increase = Increase,
                InitialDate = InitialDate,
                FinalDate = FinalDate,
                Items = Items.Select(p => new CommandItemEntity
                {
                    ProductId = p.ProductId,
                    Qtd = p.Qtd,
                    Name = p.Name,
                    Price = p.Price
                }).ToList(),
                Payments = Payments.Select(p => new PaymentEntity
                {
                    Method = p.Method,
                    Amount = p.Amount,
                    CommandId = CommandId
                }).ToList()
            };
        }
    }
}
