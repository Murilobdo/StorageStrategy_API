using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.Command
{
    public class CreateCommandValidation : CommandBaseValidation
    {
        public CreateCommandValidation()
        {
            ValidationCompanyId();
            ValidationEmployeeId();
            ValidationName();
            ValidationProducts();
        }
    }
}
