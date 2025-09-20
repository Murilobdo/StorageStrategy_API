using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands
{
    public class UpdateClientCommand : ClientCommand
    {
        public UpdateClientCommand(ClientEntity entity)
        {
            ClientId = entity.ClientId;
            CompanyId = entity.CompanyId;
            Name = entity.Name;
            Active = entity.Active;
        }

        public UpdateClientCommand()
        {
            
        }
    }
}
