using System;
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
        public List<PaymentEntity> Payments { get; set; } = new();
        
        public PaymentEnum Payment { get; set; } 
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
            PaymentEntity? payment, 
            DateTime initialDate, 
            DateTime? finalDate, 
            int companyId
        ) {
            CommandId = commandId;
            EmployeeId = employeeId;
            Name = name;
            TotalCost = totalCost;
            TotalPrice = totalPrice;
            Payments = new List<PaymentEntity>{payment};;
            InitialDate = initialDate;
            FinalDate = finalDate;
            CompanyId = companyId;
            Items = new List<CommandItemEntity>();
        }
        
        public CommandEntity(
            int commandId, 
            int employeeId, 
            string name, 
            decimal totalCost,
            decimal totalPrice, 
            List<PaymentEntity>? payments, 
            DateTime initialDate, 
            DateTime? finalDate, 
            int companyId
        ) {
            CommandId = commandId;
            EmployeeId = employeeId;
            Name = name;
            TotalCost = totalCost;
            TotalPrice = totalPrice;
            Payments = payments;
            InitialDate = initialDate;
            FinalDate = finalDate;
            CompanyId = companyId;
            Items = new List<CommandItemEntity>();
        }

        public decimal GetFinalPrice()
        {
            return TotalPrice + Increase - Discount;
        }

        public void AddDiscount(decimal discount)
        {
            if (discount > 0)
            {
                Discount += discount;
            }
        }
        
        public void AddIncrease(decimal increase)
        {
            if (increase > 0)
            {
                Increase += increase;
            }
        }

        public void FinishCommand()
        {
            FinalDate = DateTime.Now.AddHours(-3);
        }
    }
}
