using StorageStrategy.Domain.Validations.Report;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Report
{
    public record class ReadCommandsByMounthCommand : CommandBase
    {
        public int CompanyId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int EmployeeId { get; set; }

        public bool IsValid() => new ReadCommandsByMounthValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new ReadCommandsByMounthValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}