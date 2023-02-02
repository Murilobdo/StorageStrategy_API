using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;
using Xunit;

namespace StorageStrategy.Tests.Commands.Coommand
{
    public class CreateCommandCommandTest : CommandBaseTest
    {
        public CreateCommandCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_uma_comanda()
        {
            CreateCommandCommand createCommand = new(1, "Comanda Teste", 1, GetCommandItems(), PaymentEnum.Pix);

            Assert.True(createCommand.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_uma_comanda_sem_companyId()
        {
            CreateCommandCommand createCommand = new(0, "Comanda Teste", 1, GetCommandItems(), PaymentEnum.Pix);

            Assert.True(MensagemDeErroExistente(createCommand.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_comanda_sem_employeeId()
        {
            CreateCommandCommand createCommand = new(1, "Comanda Teste", 0, GetCommandItems(), PaymentEnum.Pix);

            Assert.True(MensagemDeErroExistente(createCommand.GetErros(), "O funcionário e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_comanda_sem_itens()
        {
            CreateCommandCommand createCommand = new(1, "Comanda Teste", 1, new List<CommandItemBase>(), PaymentEnum.Pix);

            Assert.True(MensagemDeErroExistente(createCommand.GetErros(), "Essa comanda não possui produtos"));
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
