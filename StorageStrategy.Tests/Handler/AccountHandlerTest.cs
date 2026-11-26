using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Options;
using Moq;
using StorageStrategy.Domain.Commands.Login;
using StorageStrategy.Domain.Handlers;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using StorageStrategy.Utils.Services;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    /// <summary>US1 (C-1): troca de senha só da própria conta, com a senha atual.</summary>
    public class AccountHandlerTest
    {
        // Valores fictícios usados só nos testes.
        private const string SenhaAtual = "SenhaAtual123";
        private const string NovaSenha = "NovaSenha456";
        private const string SenhaOutroUsuario = "OutroUsuario789";

        private readonly FakeEmployeeRepository _repo;
        private readonly AccountHandler _handler;
        private readonly EmployeeEntity _usuario;
        private readonly EmployeeEntity _outroUsuario;
        private readonly string _hashOutroUsuario;

        public AccountHandlerTest()
        {
            _repo = new FakeEmployeeRepository();
            _repo.employees.Clear();

            _usuario = new EmployeeEntity
            {
                EmployeeId = 10, CompanyId = 1, Name = "Usuario A", Email = "a@empresa1.com",
                JobRole = EmployeeRole.Manager, IsActive = true, PasswordHash = Argon2.Hash(SenhaAtual)
            };
            _hashOutroUsuario = Argon2.Hash(SenhaOutroUsuario);
            _outroUsuario = new EmployeeEntity
            {
                EmployeeId = 20, CompanyId = 2, Name = "Usuario B", Email = "b@empresa2.com",
                JobRole = EmployeeRole.Admin, IsActive = true, PasswordHash = _hashOutroUsuario
            };
            _repo.employees.Add(_usuario);
            _repo.employees.Add(_outroUsuario);

            _handler = new AccountHandler(_repo, new Mock<ICompanyRepository>().Object, Options.Create(new AppSettings()));
        }

        private static ChangePasswordCommand Comando(int userId, int companyId, string atual, string nova) =>
            new() { UserId = userId, CompanyId = companyId, CurrentPassword = atual, NewPassword = nova };

        private static bool TemErro(Result result, string mensagem) =>
            result.Errors.Any(e => e.ErrorMessage == mensagem) || result.ErrorMessage == mensagem;

        [Fact(DisplayName = "Troca de senha com a senha atual correta altera o hash e não devolve dados")]
        public async Task Sucesso_com_senha_atual_correta()
        {
            var result = await _handler.Handle(Comando(10, 1, SenhaAtual, NovaSenha), CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Senha alterada com sucesso", result.Message);
            Assert.True(Argon2.Verify(_usuario.PasswordHash, NovaSenha));
            Assert.False(Argon2.Verify(_usuario.PasswordHash, SenhaAtual));
            Assert.IsNotType<EmployeeEntity>(result.Response);
            Assert.Equal(typeof(object), result.Response.GetType());
        }

        [Fact(DisplayName = "Senha atual errada é recusada e a senha não muda")]
        public async Task Senha_atual_errada_recusada()
        {
            string hashAntes = _usuario.PasswordHash;

            var result = await _handler.Handle(Comando(10, 1, "SenhaErrada000", NovaSenha), CancellationToken.None);

            Assert.False(result.Success);
            Assert.True(TemErro(result, "Senha atual incorreta"));
            Assert.Equal(hashAntes, _usuario.PasswordHash);
        }

        [Fact(DisplayName = "Nova senha com menos de 8 caracteres é recusada")]
        public async Task Nova_senha_curta_recusada()
        {
            string hashAntes = _usuario.PasswordHash;

            var result = await _handler.Handle(Comando(10, 1, SenhaAtual, "1234567"), CancellationToken.None);

            Assert.False(result.Success);
            Assert.True(TemErro(result, "A nova senha deve ter pelo menos 8 caracteres"));
            Assert.Equal(hashAntes, _usuario.PasswordHash);
        }

        [Fact(DisplayName = "Nova senha igual à atual é recusada")]
        public async Task Nova_senha_igual_atual_recusada()
        {
            var result = await _handler.Handle(Comando(10, 1, SenhaAtual, SenhaAtual), CancellationToken.None);

            Assert.False(result.Success);
            Assert.True(TemErro(result, "A nova senha deve ser diferente da atual"));
        }

        [Fact(DisplayName = "Senha atual vazia é recusada")]
        public async Task Senha_atual_vazia_recusada()
        {
            var result = await _handler.Handle(Comando(10, 1, string.Empty, NovaSenha), CancellationToken.None);

            Assert.False(result.Success);
            Assert.True(TemErro(result, "A senha atual é obrigatória"));
        }

        [Fact(DisplayName = "Troca de senha do usuário A não altera o usuário B (isolamento)")]
        public async Task Isolamento_entre_usuarios()
        {
            var result = await _handler.Handle(Comando(10, 1, SenhaAtual, NovaSenha), CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(_hashOutroUsuario, _outroUsuario.PasswordHash);
            Assert.True(Argon2.Verify(_outroUsuario.PasswordHash, SenhaOutroUsuario));
        }

        [Fact(DisplayName = "Usuário de outra empresa não é encontrado com o CompanyId da sessão")]
        public async Task Usuario_de_outra_empresa_nao_encontrado()
        {
            var result = await _handler.Handle(Comando(20, 1, SenhaOutroUsuario, NovaSenha), CancellationToken.None);

            Assert.False(result.Success);
            Assert.True(TemErro(result, "Funcionário não encontrado"));
            Assert.Equal(_hashOutroUsuario, _outroUsuario.PasswordHash);
        }
    }
}
