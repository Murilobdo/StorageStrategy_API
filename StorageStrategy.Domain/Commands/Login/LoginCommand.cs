using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Login
{
    public record class LoginCommand : CommandBase
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public LoginCommand()
        {

        }

        public List<Error> GetErros() {
            List<Error> errors = new();

            if(string.IsNullOrEmpty(Email))
                errors.Add(new Error("Email", "O Email é obrigatório"));
            
            if(string.IsNullOrEmpty(Password))
                errors.Add(new Error("Password", "A Senha é obrigatória"));

            return errors;
        }

        public bool IsValid() => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
    }
}