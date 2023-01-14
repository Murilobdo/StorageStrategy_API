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
        public virtual List<ProductEntity> Products { get; set; } = new();
        public PaymentEnum Payment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public CompanyEntity Company { get; set; }
        public int CompanyId { get; set; }

        public CommandEntity()
        {

        }

    }
}
