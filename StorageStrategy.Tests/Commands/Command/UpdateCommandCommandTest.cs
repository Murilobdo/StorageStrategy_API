using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;
using Xunit;

namespace StorageStrategy.Tests.Commands.Coommand
{
    public class UpdateCommandCommandTest : CommandBaseTest
    {
        public UpdateCommandCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_atualizar_uma_comanda()
        {
            UpdateCommandCommand updateCommand = new(1, 1, "Comanda Teste", 1, GetCommandItems());

            Assert.True(updateCommand.IsValid());
        }

        [Fact]
        public void Erro_ao_atualizar_uma_comanda_sem_companyId()
        {
            UpdateCommandCommand updateCommand = new(1, 0, "Comanda Teste", 1, GetCommandItems());

            Assert.True(MensagemDeErroExistente(updateCommand.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_uma_comanda_sem_employeeId()
        {
            UpdateCommandCommand updateCommand = new(1, 1, "Comanda Teste", 0, GetCommandItems());

            Assert.True(MensagemDeErroExistente(updateCommand.GetErros(), "O funcionário e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_uma_comanda_sem_itens()
        {
            UpdateCommandCommand updateCommand = new(1, 1, "Comanda Teste", 1, new List<CommandItemBase>());

            Assert.True(MensagemDeErroExistente(updateCommand.GetErros(), "Essa comanda não possui produtos"));
        }

        private List<CommandItemBase> GetCommandItems() 
        {
            return new List<CommandItemBase>()
            {
                new CommandItemBase
                {
                    CommandId = 1,
                }
            };
        }

    }
}
