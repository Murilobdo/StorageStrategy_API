using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using StorageStrategy.Domain.Commands.Company;

namespace StorageStrategy.Domain.Validations.Company
{
    public class CompanyValidationBase : AbstractValidator<CompanyCommandBase>
    {
        protected void ValidationId() => RuleFor(p => p.CompanyId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatório");

        protected void ValidationName() => RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("O Nome e obrigatório");

        protected void ValidationDescription() => RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("A Descrição e obrigatório");

        protected void ValidationCreateAt() => RuleFor(p => p.CreateAt)
            .NotEmpty()
            .WithMessage("A Data de criação e obrigatório");

        protected void ValidationValidationAt() => RuleFor(p => p.Validate)
            .NotEmpty()
            .WithMessage("A Data de validade e obrigatório");
    }
}