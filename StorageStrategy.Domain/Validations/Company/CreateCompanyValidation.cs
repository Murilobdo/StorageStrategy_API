using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.Company
{
    public class CreateCompanyValidation : CompanyValidationBase
    {
        public CreateCompanyValidation()
        {
            ValidationName();
            ValidationDescription();
            ValidationCreateAt();
            ValidationValidationAt();
        }
    }
}