﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Models
{
    public class CommandEntity
    {
        public int CommandId { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeEntity Employee { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CommandItemEntity> Items { get; set; } = new();
        public decimal TotalCost { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public decimal TotalTaxing { get; set; } = 0;
        public PaymentEnum? Payment { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public CompanyEntity Company { get; set; }
        public int CompanyId { get; set; }
        public decimal Discount { get; set; }
        public decimal Increase { get; set; }

        public CommandEntity()
        {
            InitialDate = DateTime.Now;
        }

        public CommandEntity(
            int commandId, 
            int employeeId, 
            string name, 
            decimal totalCost,
            decimal totalPrice, 
            PaymentEnum? payment, 
            DateTime initialDate, 
            DateTime? finalDate, 
            int companyId
        ) {
            CommandId = commandId;
            EmployeeId = employeeId;
            Name = name;
            TotalCost = totalCost;
            TotalPrice = totalPrice;
            Payment = payment;
            InitialDate = initialDate;
            FinalDate = finalDate;
            CompanyId = companyId;
            Items = new List<CommandItemEntity>();
        }

        public decimal GetFinalPrice()
        {
            return TotalPrice - Discount + Increase;
        }
    }
}
