using StorageStrategy.Models;

namespace StorageStrategy.Models.ViewModels
{
    public record class ReportCommandViewModel(
        string Name,
        string EmployeeName,
        PaymentEnum Payment,
        DateTime FinalDate,
        List<CommandItemEntity> Items,
        decimal TotalCost,
        decimal TotalPrice,
        decimal TotalVenda
    );

    public record class ReportCommandItemViewModel(
        string Name,
        decimal Cost,
        decimal Price,
        int Qtd
    );
}