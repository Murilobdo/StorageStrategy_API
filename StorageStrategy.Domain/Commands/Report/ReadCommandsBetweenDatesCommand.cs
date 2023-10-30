using StorageStrategy.Domain.Validations.Report;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Report
{
    public record class ReadCommandsBetweenDatesCommand : CommandBase
    {
        public int CompanyId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int EmployeeId { get; set; }

        public bool IsValid() => new ReadCommandsBetweenDatesValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new ReadCommandsBetweenDatesValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}