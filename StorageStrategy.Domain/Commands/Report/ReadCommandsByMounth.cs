using StorageStrategy.Domain.Validations.Report;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Report
{
    public record class ReadCommandsByMounthCommand : CommandBase
    {
        public int CompanyId { get; set; }
        public int Month { get; set; } = 0;

        public bool IsValid() => new ReadCommandsByMounthValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new ReadCommandsByMounthValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}