using StorageStrategy.Models;

namespace StorageStrategy.Tests.Commands
{
    public abstract class CommandBaseTest
    {
        public bool MensagemDeErroExistente(List<Error> errors, string errorMessage)
        {
            if (errors.Count == 1 && errors.Select(p => p.ErrorMessage).Contains(errorMessage))
                return true;
            else
                return false;
        }
    }
}
