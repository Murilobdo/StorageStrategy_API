using StorageStrategy.Domain.Repository;

namespace StorageStrategy.Domain.Handlers.Dashboard;

public abstract class DashboardHandlerBase : HandlerBase
{
    protected ICommandRepository _repoCommand;
    protected IEmployeeRepository _repoEmployee;
    protected IExpenseRepository _repoExpenses;
    protected IProductRepository _repoProduct;

    public DashboardHandlerBase(
        ICommandRepository repoCommand,
        IExpenseRepository repoExpenses = null,
        IProductRepository repoProduct = null)
    {
        _repoCommand = repoCommand;
        _repoExpenses = repoExpenses;
        _repoProduct = repoProduct;
    }
}