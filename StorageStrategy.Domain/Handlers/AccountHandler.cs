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
        IRequestHandler<LoginCommand, Result>
    {
        private readonly IEmployeeRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;

        public AccountHandler(IEmployeeRepository repo, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _mapper = mapper;
            _appSettings = appSettings;
        }

        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            EmployeeEntity employee = await _repo.FindByEmail(request.Email);
            if(employee == null)
                return CreateError("Email ou Senha incorreta");

            if(!Argon2.Verify(employee.PasswordHash, request.Password))
                return CreateError("Email ou Senha incorreta");

            TokenService tokenService = new TokenService();
            string token = tokenService.GenerateToken(employee, _appSettings.Value.JwtKey);
            
            employee.PasswordHash = string.Empty;

            return CreateResponse(new {
                employee,
                token
            }, "Login efetuado com sucesso");
        }
    }
}