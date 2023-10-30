using StorageStrategy.Domain.Validations.Command;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command
{
    public record class AddProductCommandCommand : CommandBase, IValidation
    {
        public int CommandId { get; set; }
        public int CompanyId { get; set; }
        public List<CommandItemBase> Items { get; set; } = new();
        public AddProductCommandCommand(int commandId, int companyId, List<CommandItemBase> items)
        {
            CommandId = commandId;
            CompanyId = companyId;
            Items = items;
        }

        public AddProductCommandCommand()
        {

        }

        public List<Error> GetErros() => new AddProductCommandValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new AddProductCommandValidation().Validate(this).IsValid;
    }
}
