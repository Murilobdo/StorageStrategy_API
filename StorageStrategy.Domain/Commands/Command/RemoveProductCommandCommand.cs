using MediatR;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command;

public class RemoveProductCommandCommand : IRequest<Result>
{
    public int CommandId { get; set; }
    public int ProductId { get; set; }
    public int CompanyId { get; set; }
    public RemoveProductCommandCommand(int commandId, int productId, int companyId)
    {
        CommandId = commandId;
        ProductId = productId;
        CompanyId = companyId;
    }

    public RemoveProductCommandCommand()
    {
        
    }
}