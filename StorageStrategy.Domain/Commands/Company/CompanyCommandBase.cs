using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Commands.Company
{
    public record class CompanyCommandBase : CommandBase
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreateAt { get; set; }
        public string Validate { get; set; }
        public string CNPJ { get; set; }
        public string Phone { get; set; }

        protected CompanyCommandBase() 
        {
        }

        public CompanyCommandBase(
            int companyId, 
            string name, 
            string description, 
            bool isActive, 
            string createAt, 
            string validate, 
            string cNPJ, 
            string phone,
            string address
        ) {
            CompanyId = companyId;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreateAt = createAt;
            Validate = validate;
            CNPJ = cNPJ;
            Phone = phone;
            Address = address;
        }
    }
}
