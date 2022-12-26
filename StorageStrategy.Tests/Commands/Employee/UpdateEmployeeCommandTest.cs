using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Domain.Commands.Products;
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
            UpdateEmployeeCommand updateEmplyee = new(1, "Funcionario", 10, "Gerente", true, 1);

            Assert.True(updateEmplyee.IsValid());
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_id()
        {
            UpdateEmployeeCommand updateEmplyee = new(0, "Funcionario", 10, "Gerente", true, 1);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Id e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_nome()
        {
            UpdateEmployeeCommand updateEmplyee = new(1, string.Empty, 10, "Gerente", true, 1);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_cargo()
        {
            UpdateEmployeeCommand updateEmplyee = new(1, "Funcionario", 10, string.Empty, true, 1);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Cargo e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_funcionario_sem_companyId()
        {
            UpdateEmployeeCommand updateEmplyee = new(1, "Funcionario", 10, "Gerente", true, 0);

            Assert.True(MensagemDeErroExistente(updateEmplyee.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
