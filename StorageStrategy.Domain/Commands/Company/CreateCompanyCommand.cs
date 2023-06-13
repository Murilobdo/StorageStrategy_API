using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageStrategy.Domain.Validations.Company;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Company
{
    public record CreateCompanyCommand : CompanyCommandBase, IValidation
    {
        public CreateCompanyCommand(int companyId, string name, string description, bool isActive, 
            DateTime createAt, DateTime validate) : base(companyId, name, description, isActive, createAt, validate)
        {

        }

        public bool IsValid() => new CreateCompanyValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateCompanyValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}