namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class SalesPerEmployee
    {
        public string NameEmployee { get; set; } = string.Empty;
        public decimal TotalSales { get; set;}

        public SalesPerEmployee(string nameEmployee, decimal totalSales)
        {
            NameEmployee = nameEmployee;
            TotalSales = totalSales;
        }
    }
}
