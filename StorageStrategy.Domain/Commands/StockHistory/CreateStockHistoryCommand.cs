
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using StorageStrategy.Domain.Validations.StockHsitory;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.StockHistory
{
    public class CreateStockHistoryCommand : IValidation, IRequest<Result>
    {
        public int CompanyId { get; set; }
        public List<StockHistoryItemCommand> Products { get; set; } = new();

        public List<Error> GetErros()
        {
           return new CreateStockHsitoryValidation()
                .Validate(this)
                .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
        }

        public bool IsValid()
        {
            return new CreateStockHsitoryValidation().Validate(this).IsValid;
        }

        public List<StockHistoryItemEntity> ToEntities() =>
            Products.Select(p => new StockHistoryItemEntity
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                Cost = p.Cost,
                Price = p.Price,
                Taxing = p.Taxing
            }).ToList();
    }

    /// <summary>
    /// Item da entrada de estoque recebido pela API. Não usar StockHistoryItemEntity aqui: as
    /// propriedades de navegação dela (StockHistory, Company, ...) formam um ciclo que quebra a
    /// geração do documento OpenAPI (Scalar). Os nomes dos campos são os mesmos, então o JSON
    /// enviado pelo Site não muda.
    /// </summary>
    public class StockHistoryItemCommand
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal Taxing { get; set; }
    }
}
