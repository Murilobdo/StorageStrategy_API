using StorageStrategy.Models;
using MediatR;

namespace StorageStrategy.Domain.Commands
{
    public class UpdateClientCommand : IRequest<Result>
    {
        public int ClientId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
