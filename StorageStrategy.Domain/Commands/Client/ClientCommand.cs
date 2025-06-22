
using MediatR;
using StorageStrategy.Models;

public abstract class ClientCommand : IRequest<Result>
{
    public int ClientId { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; }
}