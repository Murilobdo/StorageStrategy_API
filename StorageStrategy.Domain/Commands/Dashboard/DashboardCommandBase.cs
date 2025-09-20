namespace StorageStrategy.Domain.Commands.Dashboard;

public abstract record DashboardCommandBase : CommandBase
{
    public int CompanyId { get; set; } = 0;
    public int Month { get; set; } = 0;
    public int Year { get; set; } = 0;
    public int? EmployeeId { get; set; }
    public int? ClientId { get; set; }
}