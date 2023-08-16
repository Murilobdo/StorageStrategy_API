namespace StorageStrategy.Domain.Commands.Report
{
    public record class ReadPaymentCommandCommand : FilterDashboard
    {
        public ReadPaymentCommandCommand(int companyId, int month, int? employeeId)
        {
            CompanyId = companyId;
            Month = month;
            EmployeeId = employeeId;
        }
    }
}
