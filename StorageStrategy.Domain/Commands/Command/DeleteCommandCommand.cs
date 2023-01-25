using FluentValidation;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Validations.Category;
using StorageStrategy.Domain.Validations.Command;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command
{
    public record class DeleteCommandCommand : CommandCommandBase, IValidation
    {
        public DeleteCommandCommand(int commandId, int companyId)
        {
            CommandId = commandId;
            CompanyId = companyId;
        }

        public List<Error> GetErros() => new DeleteCommandValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();

        public bool IsValid() => new DeleteCommandValidation().Validate(this).IsValid;
    }
}
