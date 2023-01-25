using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Command;
using Xunit;

namespace StorageStrategy.Tests.Commands.Coommand
{
    public class DeleteCommandCommandTest : CommandBaseTest
    {
        public DeleteCommandCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_excluir_uma_comanda()
        {
            DeleteCommandCommand deleteCommand = new(commandId: 1, companyId: 1);

            Assert.True(deleteCommand.IsValid());
        }

        [Fact]
        public void Erro_ao_excluir_uma_comanda_sem_id()
        {
            DeleteCommandCommand deleteCommand = new(commandId: 0, companyId: 1);

            Assert.True(MensagemDeErroExistente(deleteCommand.GetErros(), "O Id e obrigatório"));
        }

        [Fact]
        public void Erro_ao_excluir_uma_comanda_sem_id_empresa()
        {
            DeleteCommandCommand deleteCommand = new(commandId: 1, companyId: 0);

            Assert.True(MensagemDeErroExistente(deleteCommand.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
