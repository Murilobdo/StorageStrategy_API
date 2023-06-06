using FluentValidation;
using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Validations.Employee
{
    public class EmployeeValidationBase : AbstractValidator<EmployeeCommandBase>
    {
        protected void ValidationId() => RuleFor(p => p.EmployeeId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatório");

        protected void ValidationCompanyId() => RuleFor(p => p.CompanyId)
           .GreaterThan(0)
           .WithMessage("O Id da empresa e obrigatório");

        protected void ValidationName() => RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("O Nome e obrigatório");

        protected void ValidationPassword() => RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("A Senha e obrigatório");
        
        protected void ValidationEmail() => RuleFor(p => p.Email)
            .EmailAddress()
            .WithMessage("O Email e obrigatório");

        protected void ValidationComission() => RuleFor(p => p.Comission)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O Comissão e obrigatório");

        protected void ValidationJobRole() => RuleFor(p => p.JobRole)
            .NotEmpty()
            .WithMessage("O Cargo e obrigatório");
    }
}
