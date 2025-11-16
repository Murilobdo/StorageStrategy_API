using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using StorageStrategy.Domain.AutoMapper;
using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Domain.Handlers;
using StorageStrategy.Tests.FakeRepository;
using System;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class EmployeeHandlerTest : HandlerBaseTest
    {
        private EmployeeHandler _handler;
        private FakeEmployeeRepository _repo;
        private IMapper _mapper;
        private CancellationToken _cancellationToken;
        private ILoggerFactory _log;


        public EmployeeHandlerTest()
        {
            _cancellationToken = new CancellationToken();
            _repo = new FakeEmployeeRepository();

            MapperConfigurationExpression cfg = new MapperConfigurationExpression();
            cfg.AddProfile(new EmployeeProfile());
            _log = new Mock<ILoggerFactory>().Object;

            _mapper = new MapperConfiguration(cfg).CreateMapper();

            _handler = new EmployeeHandler(
                _repo,
                _mapper
            );
        }

        #region CREATE EMPLOYEE
        [Fact(DisplayName = "Sucesso ao Criar um Funcionario")]
        public async Task Sucesso_ao_criar_um_funcionario()
        {
            CreateEmployeeCommand create = _repo.CreateEmployeeCommand;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro ao Criar um Funcionario Com Nome Existente")]
        public async Task Erro_ao_criar_um_funcionario_com_nome_existente()
        {
            CreateEmployeeCommand create = _repo.CreateEmployeeCommand;

            create.Email = _repo.employees[0].Email;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.False(IsValid(result, "Ja existe um funcionário com esse nome"));
        }
        #endregion

        #region UPDATE EMPLOYEE
        [Fact(DisplayName = "Sucesso ao Atualizar um Funcionario")]
        public async Task Sucesso_ao_atualizar_um_funcionario()
        {
            UpdateEmployeeCommand create = _repo.UpdateEmployeeCommand;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro ao Atualizar um Funcionario Inexistente")]
        public async Task Erro_ao_atualizar_um_funcionario_inexistente()
        {
            CreateEmployeeCommand create = _repo.CreateEmployeeCommand;

            create.CompanyId = 999;
            create.EmployeeId = 888;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.False(IsValid(result, "Funcionario não encontrado para a atualização"));
        }
        #endregion

        #region DELETE EMPLOYEE
        [Fact(DisplayName = "Sucesso ao Deletar um Funcionario")]
        public async Task Sucesso_ao_deletar_um_funcionario()
        {
            DeleteEmployeeCommand create = _repo.DeleteEmployeeCommand;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro ao Atualizar um Funcionario Com Nome Existente")]
        public async Task Erro_ao_deletar_um_funcionario_existente()
        {
            CreateEmployeeCommand create = _repo.CreateEmployeeCommand;

            create.CompanyId = 999;
            create.EmployeeId = 888;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.False(IsValid(result, "Funcionario não encontrado para a exclusão"));
        }
        #endregion

    }

}