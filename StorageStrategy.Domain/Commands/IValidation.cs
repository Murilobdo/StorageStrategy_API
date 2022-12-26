using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands
{
    public interface IValidation
    {
        public bool IsValid();
        public List<Error> GetErros();
    }
}
