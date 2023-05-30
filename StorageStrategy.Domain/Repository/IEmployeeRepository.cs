using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface IEmployeeRepository : IRepositoryBase<EmployeeEntity>
    {
        Task<EmployeeEntity> FindByName(string name, int companyId);
        Task<List<EmployeeEntity>> ToList(int companyId);
        Task<EmployeeEntity> GetByIdAsync(int employeeId, int companyId);
        Task<EmployeeEntity> FindByEmail(string email);
    }
}
