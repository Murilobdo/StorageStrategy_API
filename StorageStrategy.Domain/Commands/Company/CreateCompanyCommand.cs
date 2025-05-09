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

        public CreateCompanyCommand(
            int companyId, 
            string name, 
            string description, 
            bool isActive, 
            string createAt,
            string validate, 
            string cNPJ, 
            string phone,
            string address,
            string adminUserEmail,
            string adminUserName,
            string password
        ) {
            CompanyId = companyId;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreateAt = createAt;
            Validate = validate;
            CNPJ = cNPJ;
            Phone = phone;
            AdminUserEmail = adminUserEmail;
            AdminUserName = adminUserName;
            Password = password;
            Address = address;
        }

        public string AdminUserEmail { get; set; }
        public string AdminUserName { get; set; }
        public string Password { get; set; }   

        public bool IsValid() => new CreateCompanyValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateCompanyValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}