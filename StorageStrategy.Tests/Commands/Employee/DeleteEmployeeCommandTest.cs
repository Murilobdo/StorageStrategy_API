using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Domain.Commands.Products;
using Xunit;

namespace StorageStrategy.Tests.Commands.Employee
{
    public class DeleteEmployeeCommandTest : CommandBaseTest
    {
        public DeleteEmployeeCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_excluir_um_funcionario()
        {
            DeleteEmployeeCommand deleteEmplyee = new(1, 1);

            Assert.True(deleteEmplyee.IsValid());
        }

        [Fact]
        public void Erro_ao_excluir_um_funcionario_sem_id()
        {
            DeleteEmployeeCommand deleteEmplyee = new(0, 1);

            Assert.True(MensagemDeErroExistente(deleteEmplyee.GetErros(), "O Id e obrigatório"));
        }

        [Fact]
        public void Erro_ao_excluir_um_funcionario_sem_companyId()
        {
            DeleteEmployeeCommand deleteEmplyee = new(1, 0);

            Assert.True(MensagemDeErroExistente(deleteEmplyee.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
