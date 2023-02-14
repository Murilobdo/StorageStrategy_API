using StorageStrategy.Domain.Commands.Dashboard;
using Xunit;

namespace StorageStrategy.Tests.Commands.Dashboard
{
    public class EntryAndExitOfMonthCommandTest : CommandBaseTest
    {
        public EntryAndExitOfMonthCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_EntryAndExitOfMonthCommand()
        {
            EntryAndExitOfMonthCommand command = new(1, new DateTime());

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_EntryAndExitOfMonthCommandTest_sem_companyId()
        {
            EntryAndExitOfMonthCommand command = new(0, new DateTime());

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da Empresa e obrigatório"));
        }
    }
}
