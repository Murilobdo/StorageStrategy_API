namespace StorageStrategy.Models.ViewModels.Report;

public class ReportCommandViewModel
{
    public List<CommandEntity> Commands { get; set; }
    public decimal TotalCostService { get; set; }
    public decimal TotalCostProduct { get; set; }
    public decimal TotalPriceService { get; set; }
    public decimal TotalPriceProduct { get; set; }
}