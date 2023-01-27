using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.Command
{
    public class UpdateCommandValidation : CommandBaseValidation
    {
        public UpdateCommandValidation()
        {
            ValidationId();
            ValidationCompanyId();
            ValidationEmployeeId();
            ValidationName();
            ValidationProducts();
        }
    }
}
