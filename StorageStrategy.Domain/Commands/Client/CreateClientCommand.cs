
using StorageStrategy.Models;

public class CreateClientCommand : ClientCommand
{
    public CreateClientCommand(ClientEntity entity)
    {
        CompanyId = entity.CompanyId;
        ClientId = entity.ClientId;
        Name = entity.Name;
    }

    public CreateClientCommand()
    {
        
    }
}