using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command
{
    public record class CommandCommandBase : CommandBase
    {
        public CommandCommandBase(
            int commandId, 
            int companyId, 
            string name, 
            int employeeId, 
            decimal discount,
            decimal increase,
            List<CommandItemBase> items, 
            List<PaymentCommand> payments
        ) {
            CommandId = commandId;
            CompanyId = companyId;
            Name = name;
            EmployeeId = employeeId;
            Items = items;
            Payments = payments;
            Discount = discount;
            Increase = increase;
        }
        
        public CommandCommandBase(
            int commandId, 
            int companyId, 
            string name, 
            int employeeId, 
            decimal discount,
            decimal increase,
            List<CommandItemBase> items, 
            PaymentCommand? payment
        ) {
            CommandId = commandId;
            CompanyId = companyId;
            Name = name;
            EmployeeId = employeeId;
            Items = items;
            Payments = new List<PaymentCommand>{payment};
            Discount = discount;
            Increase = increase;
        }
        
        public CommandCommandBase()
        {

        }

        public int CommandId { get; set; }
        public int CompanyId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public List<CommandItemBase> Items { get; set; } = new();
        public List<PaymentCommand> Payments { get; set; } = new();
        public decimal Discount { get; set; }
        public decimal Increase { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
