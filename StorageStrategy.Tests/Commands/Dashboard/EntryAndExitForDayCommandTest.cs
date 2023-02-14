using StorageStrategy.Domain.Commands.Dashboard;
using Xunit;

namespace StorageStrategy.Tests.Commands.Dashboard
{
    public class EntryAndExitForDayCommandTest : CommandBaseTest
    {
        public EntryAndExitForDayCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_EntryAndExitForDayCommand()
        {
            EntryAndExitForDayCommand command = new(1, new DateTime());

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_EntryAndExitForDayCommand_sem_companyId()
        {
            EntryAndExitForDayCommand command = new(0, new DateTime());

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da Empresa e obrigatório"));
        }
    }
}
