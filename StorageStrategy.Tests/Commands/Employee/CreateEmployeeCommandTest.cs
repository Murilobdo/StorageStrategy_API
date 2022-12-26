using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Domain.Commands.Products;
using Xunit;

namespace StorageStrategy.Tests.Commands.Employee
{
    public class CreateEmployeeCommandTest : CommandBaseTest
    {
        public CreateEmployeeCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_um_funcionario()
        {
            CreateEmployeeCommand createEmplyee = new(1, "Funcionario", 10, "Gerente", true, 1);

            Assert.True(createEmplyee.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_um_funcionario_sem_nome()
        {
            CreateEmployeeCommand createEmplyee = new(1, string.Empty, 10, "Gerente", true, 1);

            Assert.True(MensagemDeErroExistente(createEmplyee.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_um_funcionario_sem_cargo()
        {
            CreateEmployeeCommand createEmplyee = new(1, "Funcionario", 10, string.Empty, true, 1);

            Assert.True(MensagemDeErroExistente(createEmplyee.GetErros(), "O Cargo e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_um_funcionario_sem_companyId()
        {
            CreateEmployeeCommand createEmplyee = new(1, "Funcionario", 10, "Gerente", true, 0);

            Assert.True(MensagemDeErroExistente(createEmplyee.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
