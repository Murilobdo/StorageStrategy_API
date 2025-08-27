using MediatR;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Client;

public record GetClientsTotalCommandQuery(int CompanyId) : IRequest<Result>
{
}