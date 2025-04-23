using MediatR;
using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Commands.NFE
{
    public class SearchNFQuery : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
