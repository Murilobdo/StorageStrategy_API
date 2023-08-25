using StorageStrategy.Domain.Commands.Dashboard;
using Xunit;

namespace StorageStrategy.Tests.Commands.Dashboard
{
    public class InfoPaymentCommandTest : CommandBaseTest
    {
        public InfoPaymentCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_InfoPaymentCommandTest()
        {
            InfoPaymentCommand command = new(1, 1);

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_InfoPaymentCommandTest_sem_companyId()
        {
            InfoPaymentCommand command = new(0, 1);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da Empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_InfoPaymentCommandTest_sem_mes()
        {
            InfoPaymentCommand command = new(1, 0);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Mês e obrigatório"));
        }
    }
}
