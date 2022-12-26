using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Repository
{
    public interface IEmployeeRepository : IRepositoryBase<EmployeeEntity>
    {
        Task<EmployeeEntity> FindByName(string name, int companyId);
        Task<List<EmployeeEntity>> ToList(int companyId);
        Task<EmployeeEntity> GetByIdAsync(int employeeId, int companyId);
    }
}
