using AutoMapper;
using Isopoh.Cryptography.Argon2;
using MediatR;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Commands.Login;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Handlers
{
    
    public class AccountHandler : HandlerBase,
        IRequestHandler<LoginCommand, Result>,
        IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IEmployeeRepository _repo;
        private readonly ICompanyRepository _repoCompany;
        private readonly IOptions<AppSettings> _appSettings;

        public AccountHandler(
            IEmployeeRepository repo, 
            ICompanyRepository repoCompany,
            IOptions<AppSettings> appSettings
        ) {
            _repo = repo;
            _appSettings = appSettings;
            _repoCompany = repoCompany;
        }

        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            EmployeeEntity employee = await _repo.FindByEmail(request.Email);

            // if (employee == null)
            //     return CreateError("Email ou Senha incorreta");
            //
            // CompanyEntity company = await _repoCompany.GetById(employee.CompanyId);
            //
            // if (company.Validate <= DateTime.Now)
            //     return CreateError(
            //         $"Sua licenca expirou dia {company.Validate.ToShortDateString()}, entre em contato para renovação");

            // if (!Argon2.Verify(employee.PasswordHash, request.Password))
            //     return CreateError("Email ou Senha incorreta");

            TokenService tokenService = new TokenService();
            string token = tokenService.GenerateToken(employee, _appSettings.Value.JwtKey);

            employee.PasswordHash = string.Empty;

            return CreateResponse(new
            {
                employee,
                token
            }, "Login efetuado com sucesso");
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repo.GetByIdAsync(request.UserId, request.CompanyId);

            if(employee == null)
                return CreateError("Funcionário não encontrado");

            employee.PasswordHash = Argon2.Hash(request.NewPassword);

            _repo.Update(employee);
            await _repo.SaveAsync();

            return CreateResponse(employee, "Senha alterada com sucesso");
        }
    }
}