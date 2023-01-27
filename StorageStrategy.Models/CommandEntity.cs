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
        public virtual EmployeeEntity Employee { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List<CommandItem> Items { get; set; } = new();
        public decimal TotalCost { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public PaymentEnum? Payment { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public CompanyEntity Company { get; set; }
        public int CompanyId { get; set; }

        public CommandEntity()
        {
            InitialDate = DateTime.Now;
        }

    }
}
