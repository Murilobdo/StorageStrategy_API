using AutoMapper;
using Isopoh.Cryptography.Argon2;
using MediatR;
using StorageStrategy.Domain.Commands.Company;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Helpers;
using System.ComponentModel.Design;

namespace StorageStrategy.Domain.Handlers
{
    public class CompanyHandler : HandlerBase,
        IRequestHandler<CreateCompanyCommand, Result>
    {

        private readonly ICompanyRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public CompanyHandler(ICompanyRepository repository, IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid())
                return CreateError(request.GetErros(), "Dados inválidos");

            var company = _mapper.Map<CompanyEntity>(request);

            await _repository.AddAsync(company);
            await _repository.SaveAsync();

            await CreateUserCompanyAdmin(request.AdminUserName, request.AdminUserEmail, request.Password, company.CompanyId);
            await _repository.CreateCompanyExpenseCategorys(InitialData.GetExpensesCategories(company.CompanyId));
            
            await _repository.SaveAsync();

            return CreateResponse(company, "Empresa cadastrada com sucesso.");
        }

        private async Task CreateUserCompanyAdmin(string adminUserName, string adminUserEmail, string password, int companyId)
        {
            var employeeAdmin = new EmployeeEntity
            {
                CompanyId = companyId,
                Email = adminUserEmail,
                Name = adminUserName,
                PasswordHash = Argon2.Hash(password),
                IsActive = true,
                JobRole = "Gerente",
                Comission = 0
            };

            await _employeeRepository.AddAsync(employeeAdmin);
            await _employeeRepository.SaveAsync();
        }
    }
}