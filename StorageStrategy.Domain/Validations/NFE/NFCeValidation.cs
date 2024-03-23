using FluentValidation;
using StorageStrategy.Domain.Commands.NFE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.NFE
{
    public class NFCeValidation : AbstractValidator<CreateNFCommand>
    {
        public NFCeValidation()
        {
            ValidationCompanyId();
        }

        protected void ValidationCompanyId() => RuleFor(p => p.CompanyId).NotEmpty();
    }
}
