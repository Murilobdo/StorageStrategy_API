using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class DRECommand : DashboardCommandBase
    {
        public DRECommand() 
        { }

        public decimal ReceitaBruta { get; set; }
        public decimal DeducoesAbatimentos { get; set; }
        public decimal ReceitaLiquida { get; set; }
        public decimal CPV { get; set; }
        public decimal LucroBruto { get; set; }
        public decimal DespesasAdministrativas { get; set; }
        public decimal ResultadoLiquido { get; set; }

        public List<Error> GetErros() => new DREValidation().Validate(this)
         .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new DREValidation().Validate(this).IsValid;
    }
}
