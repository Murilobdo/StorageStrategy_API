using StorageStrategy.Domain.Commands.Command;
using Xunit;

namespace StorageStrategy.Tests.Commands.Command
{
    public class AddProductCommandTest : CommandBaseTest
    {
        public AddProductCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_adicionar_item_na_comanda()
        {
            AddProductCommandCommand addProductCommand = new(1, 1, GetItem());

            Assert.True(addProductCommand.IsValid());   
        }

        [Fact]
        public void Erro_ao_adicionar_item_na_comanda_sem_commandId()
        {
            AddProductCommandCommand addProductCommand = new(0, 1, GetItem());

            Assert.True(MensagemDeErroExistente(addProductCommand.GetErros(), "O id da comanda e obrigatorio"));
        }

        [Fact]
        public void Erro_ao_adicionar_item_na_comanda_sem_companyId()
        {
            AddProductCommandCommand addProductCommand = new(1, 0, GetItem());

            Assert.True(MensagemDeErroExistente(addProductCommand.GetErros(), "O id da empresa e obrigatorio"));
        }

        [Fact]
        public void Erro_ao_adicionar_um_item_vazio_na_comanda()
        {
            AddProductCommandCommand addProductCommand = new(1, 1, new List<CommandItemBase>());

            Assert.True(MensagemDeErroExistente(addProductCommand.GetErros(), "A comanda deve possuir itens"));
        }

        private List<CommandItemBase> GetItem() 
        {
            return new List<CommandItemBase>()
            {
                new CommandItemBase()
            };
        }
    }
}
