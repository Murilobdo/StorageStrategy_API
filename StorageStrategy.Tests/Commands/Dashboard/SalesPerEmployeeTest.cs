using StorageStrategy.Domain.Commands.Dashboard;
using Xunit;

namespace StorageStrategy.Tests.Commands.Dashboard
{
    public class SalesPerEmployeeTest : CommandBaseTest
    {

        [Fact]
        public void Sucesso_ao_criar_SalesPerEmployeeCommand() 
        {
            SalesPerEmployeeCommand command = new(companyId: 1, month: 5, 2025);

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_SalesPerEmployeeCommand_sem_companyId()
        {
            SalesPerEmployeeCommand command = new(companyId: 0, month: 5, 2025);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da Empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_SalesPerEmployeeCommand_sem_month()
        {
            SalesPerEmployeeCommand command = new(companyId: 1, month: 0, 2025);
            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Mês e obrigatório"));
        }
    }
}
