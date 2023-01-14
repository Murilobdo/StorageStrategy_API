using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Models;
using System.ComponentModel.Design;

namespace StorageStrategy.Domain.Commands.Command
{
    public record class CommandCommandBase : CommandBase
    {
        public CommandCommandBase(int commandId, int companyId, string name, int employeeId, List<ProductCommandBase> products, PaymentEnum payment)
        {
            CommandId = commandId;
            CompanyId = companyId;
            Name = name;
            EmployeeId = employeeId;
            Products = products;
            Payment = payment;
        }
        public CommandCommandBase()
        {

        }

        public int CommandId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public List<ProductCommandBase> Products { get; set; } = new();
        public PaymentEnum Payment { get; set; }
    }
}
