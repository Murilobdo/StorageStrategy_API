using MediatR;
using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Commands.Company
{
    public record InactivateCompanyCommand(int CompanyId) : IRequest<Result>
    {
        public int CompanyId { get; set; } = CompanyId;
    }
}
