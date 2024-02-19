
using FluentValidation;
using StorageStrategy.Domain.Commands.StockHistory;

namespace StorageStrategy.Domain.Validations.StockHsitory
{
    public class CreateStockHsitoryValidation : AbstractValidator<CreateStockHsitoryCommand>
    {
        public CreateStockHsitoryValidation()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty()
                .WithMessage("O Id da empresa e obrigatório");

            RuleFor(x => x.Products.Count)
                .GreaterThan(0)
                .WithMessage("Selecione pelo menos um produto");
        }
    }
}
