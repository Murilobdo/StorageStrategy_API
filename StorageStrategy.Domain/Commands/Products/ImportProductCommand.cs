using MediatR;
using StorageStrategy.Models.ViewModels;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Products
{
    public record ImportProductCommand : IRequest<Result>
    {
        public List<ImportProductViewModel> Products { get; set; } = new();
        public int CompanyId { get; set; }
    }
}