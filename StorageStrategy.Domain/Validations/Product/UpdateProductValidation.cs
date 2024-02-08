using StorageStrategy.Domain.Commands;
using StorageStrategy.Domain.Commands.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.Product
{
    public class UpdateProductValidation : ProductValidationBase
    {
        public UpdateProductValidation()
        {
            ValidationId();
            ValidationCategoryId();
            ValidationCompanyId();
            ValidationName();
            ValidationCost();
            ValidationPrice();
            ValidationQtd();
            ValidationTaxing();
        }
    }
}
