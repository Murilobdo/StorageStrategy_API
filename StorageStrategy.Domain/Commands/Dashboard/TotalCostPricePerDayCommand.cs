using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class TotalCostPricePerDayCommand : CommandBase
    {
        public int CompanyId { get; set; }
        public int Month { get; set; }

        public TotalCostPricePerDayCommand()
        {

        }

        public TotalCostPricePerDayCommand(int companyId, int month)
        {
            CompanyId = companyId;
            Month = month;
        }

        public List<Error> GetErros() => new TotalCostPricePerDayValidation().Validate(this)
           .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();

        public bool IsValid() => new TotalCostPricePerDayValidation().Validate(this).IsValid;
    }
}
