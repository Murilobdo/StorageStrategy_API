using StorageStrategy.Models;

namespace StorageStrategy.Models.ViewModels
{
    public record class ImportProductViewModel(
        string Name,
        string Category,
        string Cost,
        string Price,
        string Qtd,
        string StockAlert,
        int CompanyId,
        decimal Taxing
    );
}