using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Tests.FakeRepository
{
    public class FakeEmployeeRepository : IEmployeeRepository
    {
        public List<EmployeeEntity> employees;

        public CreateEmployeeCommand CreateEmployeeCommand; 
        public UpdateEmployeeCommand UpdateEmployeeCommand; 
        public DeleteEmployeeCommand DeleteEmployeeCommand; 

        public FakeEmployeeRepository()
        {
            employees = new()
            {
                new EmployeeEntity(1, "Employee 1", "employee@email.com", 10, EmployeeRole.Manager, true, "H4SH3S4NH4", 1),
                new EmployeeEntity(1, "Employee 2", "employee@email.com", 5, EmployeeRole.Employee, true, "H4SH3S4NH4", 1),
                new EmployeeEntity(1, "Employee 3", "employee@email.com", 8, EmployeeRole.Employee, true, "H4SH3S4NH4", 1),
                new EmployeeEntity(1, "Employee 4", "employee@email.com", 3, EmployeeRole.Employee, true, "H4SH3S4NH4", 1)
            };

            CreateEmployeeCommand = new(name: "Funcionario", comission: 5, jobRole: EmployeeRole.Employee, email: "funcionario@email.com", password: "hash", isActive: true, companyId: 1);
            UpdateEmployeeCommand = new(employeeId: 1, name: "Funcionario", comission: 5, jobRole: EmployeeRole.Employee, email: "funcionario@email.com", password: "hash", isActive: true, companyId: 1);
            DeleteEmployeeCommand = new(employeeId: 1, companyId: 1);
        }

        public Task AddAsync(EmployeeEntity model)
        {
            employees.Add(model);
            return Task.CompletedTask;
        }

        public Task<List<EmployeeEntity>> GetAllAsync(Func<EmployeeEntity, bool> filter, string[] includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public Task CreateTranscationAsync()
        {
            return Task.CompletedTask;
        }

        public void Delete(EmployeeEntity id)
        {
        }

        public Task<EmployeeEntity> FindByEmail(string email)
        {
            return Task.FromResult(employees.FirstOrDefault(p => p.Email.Equals(email)));
        }

        public Task<EmployeeEntity> FindByName(string name, int companyId)
        {
            return Task.FromResult(employees.FirstOrDefault(p => p.Email.Equals(name) && p.CompanyId == companyId));
        }
        public Task<EmployeeEntity> GetById(int id)
        {
            return Task.FromResult(employees.FirstOrDefault(p => p.EmployeeId == id));
        }

        public Task<EmployeeEntity> GetByIdAsync(int employeeId, int companyId)
        {
            return Task.FromResult(employees.FirstOrDefault(p => p.EmployeeId == employeeId && p.CompanyId == companyId));
        }

        public void RemoveRange(EmployeeEntity model)
        {
            employees.Remove(model);
        }

        public Task RollbackAsync()
        {
            return Task.CompletedTask;
        }

        public void Save()
        {
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }

        public Task<List<EmployeeEntity>> ToList(int companyId)
        {
            return Task.FromResult(employees);
        }

        public void Update(EmployeeEntity model)
        {
        }
    }
}
