using StorageStrategy.Domain.Validations.Report;
using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Commands.Report
{
    public abstract record FilterDashboard : CommandBase
    {
         public int CompanyId { get; set; }
        public int Month { get; set; }
        public int? EmployeeId { get; set; }

        public bool IsValid() => new ReadCommandsByMounthValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new ReadCommandsByMounthValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
