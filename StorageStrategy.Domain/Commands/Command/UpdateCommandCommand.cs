﻿using StorageStrategy.Domain.Validations.Command;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command
{
    public record class UpdateCommandCommand : CommandCommandBase, IValidation
    {
        public UpdateCommandCommand(int commandId, int companyId, string name, int employeeId, List<CommandItemBase> items)
        {
            CommandId = commandId;
            CompanyId = companyId;
            Name = name;
            EmployeeId = employeeId;
            Items = items;
        }

        public List<Error> GetErros() => new CreateCommandValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new CreateCommandValidation().Validate(this).IsValid;
    }
}
