﻿using StorageStrategy.Models;

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
            PaymentEnum payment
        ) {
            CommandId = commandId;
            CompanyId = companyId;
            Name = name;
            EmployeeId = employeeId;
            Items = items;
            Payment = payment;
            Discount = discount;
            Increase = increase;
        }
        public CommandCommandBase()
        {

        }

        public int CommandId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public List<CommandItemBase> Items { get; set; } = new();
        public PaymentEnum? Payment { get; set; }
        public decimal Discount { get; set; }
        public decimal Increase { get; set; }
    }
}
