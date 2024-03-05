
using MediatR;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Company
{
    public record class RenovateCompanyCommand(int CompanyId, string NewDate) : IRequest<Result>
    {
        public int CompanyId { get; set; } = CompanyId;
        public string NewDate { get; set; } = NewDate;
    }
}
