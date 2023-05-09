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
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Validate { get; set; }

        protected CompanyCommandBase() 
        {
        }

        public CompanyCommandBase(int companyId, string name, string description, bool isActive, DateTime createAt, DateTime validate)
        {
            CompanyId = companyId;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreateAt = createAt;
            Validate = validate;
        }
    }
}
