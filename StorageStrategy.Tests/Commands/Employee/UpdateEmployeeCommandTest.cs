using StorageStrategy.Domain.Commands.Employee;
using Xunit;

namespace StorageStrategy.Tests.Commands.Employee
{
    public class UpdateEmployeeCommandTest : CommandBaseTest
    {
        public UpdateEmployeeCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_atualizar_um_funcionario()
        {
            UpdateEmployeeCommand updateEmplyee = new(1, "Nome", 10, "Gerente", "email@teste.com", "Senha", true, 1);
            Assert.True(updateEmplyee.IsValid());
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_id()
        {
            UpdateEmployeeCommand updateEmplyee = new(0, "Nome", 10, "Gerente", "email@teste.com", "Senha", true, 1);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Id e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_nome()
        {
            UpdateEmployeeCommand updateEmplyee = new(1, string.Empty, 10, "Gerente", "email@teste.com",  "Senha", true, 1);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_cargo()
        {
            UpdateEmployeeCommand updateEmplyee = new(1, "Funcionario", 10, string.Empty, "email@teste.com", "Senha", true, 1);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Cargo e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_email()
        {
            UpdateEmployeeCommand updateEmplyee = new(1, "Funcionario", 10, "Gerente", string.Empty, "Senha", true, 1);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Email e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_companyId()
        {
            UpdateEmployeeCommand updateEmplyee = new(1, "Funcionario", 10, "Gerente", "email@teste.com", "Senha", true, 0);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
