using AutoMapper;
using Isopoh.Cryptography.Argon2;
using MediatR;
using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class EmployeeHandler : HandlerBase,
        IRequestHandler<CreateEmployeeCommand, Result>,
        IRequestHandler<UpdateEmployeeCommand, Result>,
        IRequestHandler<DeleteEmployeeCommand, Result>
    {

        private readonly IEmployeeRepository _repo;
        private readonly IMapper _mapper;

        public EmployeeHandler(IEmployeeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var employee = await _repo.FindByName(request.Name, request.CompanyId);

            if (employee is not null)
                return CreateError("Ja existe um funcionário com esse nome.");

            employee = _mapper.Map<EmployeeEntity>(request);
            employee.PasswordHash = Argon2.Hash(request.Password);
            
            await _repo.AddAsync(employee);
            await _repo.SaveAsync();

            return CreateResponse(employee, "Funcionario cadastrado com sucesso.");
        }

        public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var employee = await _repo.GetByIdAsync(request.EmployeeId, request.CompanyId);

            if (employee is null)
                return CreateError("Funcionario não encontrado para a atualização.");

            employee = _mapper.Map<EmployeeEntity>(request);

            _repo.Update(employee);
            await _repo.SaveAsync();

            return CreateResponse(employee, "Funcionario atualizado com sucesso.");
        }

        public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var employee = await _repo.GetByIdAsync(request.EmployeeId, request.CompanyId);

            if (employee is null)
                return CreateError("Funcionario não encontrado para a exclusão.");

            employee = _mapper.Map<EmployeeEntity>(request);

            _repo.Delete(employee);
            await _repo.SaveAsync();

            return CreateResponse(employee, "Funcionario excluido com sucesso.");
        }
    }
}
