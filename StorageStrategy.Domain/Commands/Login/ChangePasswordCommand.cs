using System.Text.Json.Serialization;
using StorageStrategy.Domain.Validations.Account;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Login
{
    public record class ChangePasswordCommand : CommandBase, IValidation
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;

        // Identidade sempre preenchida pelo controller a partir do token; nunca aceita do corpo.
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int CompanyId { get; set; }

        public bool IsValid()
        {
            return new ChangePasswordValidation().Validate(this).IsValid;
        }

        public List<Error> GetErros()
        {
            return new ChangePasswordValidation().Validate(this)
                .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
        }
    }
}
