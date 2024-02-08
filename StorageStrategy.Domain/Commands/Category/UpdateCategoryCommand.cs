﻿using StorageStrategy.Domain.Validations.Category;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Category
{
    public record class UpdateCategoryCommand : CategoryCommandBase, IValidation
    {
        public UpdateCategoryCommand(int categoryId, string name, bool isActive, int companyId)
        {
            CategoryId = categoryId;
            Name = name;
            IsActive = isActive;
            CompanyId = companyId;
        }
        public UpdateCategoryCommand()
        {
        }

        public bool IsValid() => new UpdateCategoryValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new UpdateCategoryValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
