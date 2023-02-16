using StorageStrategy.Domain.Commands.Expenses;
using Xunit;

namespace StorageStrategy.Tests.Commands.Extenses
{
    public class CreateExpensesCommandTest : CommandBaseTest
    {
        public CreateExpensesCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_uma_despesa()
        {
            CreateExpensesCommand command = new(1, "Description", 1, DateTime.Now);

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_uma_despesa_companyId()
        {
            CreateExpensesCommand command = new(0, "Description", 1, DateTime.Now);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_despesa_descricao()
        {
            CreateExpensesCommand command = new(1, string.Empty, 1, DateTime.Now);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "A Descrição e obrigatória"));
        }

        [Fact]
        public void Erro_ao_criar_uma_despesa_categoria()
        {
            CreateExpensesCommand command = new(1, "Description", 0, DateTime.Now);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "A caregoria da despesa e obrigatória"));
        }
    }
}
