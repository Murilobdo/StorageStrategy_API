using MediatR;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands
{
    public record class CommandBase : IRequest<Result>
    {
    }
}
