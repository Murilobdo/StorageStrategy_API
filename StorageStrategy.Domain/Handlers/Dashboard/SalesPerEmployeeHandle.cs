
using MediatR;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Domain.Handlers.Dashboard;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

public class SalesPerEmployeeHandle : DashboardHandlerBase, IRequestHandler<SalesPerEmployeeCommand, Result>
{
    public SalesPerEmployeeHandle(ICommandRepository repoCommand, IEmployeeRepository repoEmployee) : base(repoCommand)
    {
    }

    public async Task<Result> Handle(SalesPerEmployeeCommand request, CancellationToken cancellationToken)
    {
        List<SalesPerEmployee> response = new();

        if(!request.IsValid())
            return CreateError(request.GetErros(), "Dados Inválidos");

        List<CommandEntity> commands = await _repoCommand.ReadMonthCommandsAsync(request.CompanyId, request.Month, request.Year);

        List<EmployeeEntity> employees = await _repoEmployee.ToList(request.CompanyId);

        foreach (var employee in employees)
        {
            var totalSales = commands
                .Where(p => p.EmployeeId == employee.EmployeeId)
                .Sum(p => p.TotalPrice);

            response.Add(new SalesPerEmployee(employee.Name, totalSales));
        }

        return CreateResponse(response, "Busca realizada");
    }
}