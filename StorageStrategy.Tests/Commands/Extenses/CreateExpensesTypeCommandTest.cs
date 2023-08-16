using StorageStrategy.Domain.Commands.Expenses;
using Xunit;

namespace StorageStrategy.Tests.Commands.Extenses
{
    public class CreateExpensesTypeCommandTest : CommandBaseTest
    {
        public CreateExpensesTypeCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_uma_categoria_de_despesa()
        {
            CreateExpenseTypeCommand command = new(1, "Description");

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_uma_categoria_de_despesa_sem_companyId()
        {
            CreateExpenseTypeCommand command = new(0, "Description");

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_categoria_de_despesa_sem_descricao()
        {
            CreateExpenseTypeCommand command = new(1, string.Empty);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "A Descrição e obrigatória"));
        }
    }
}
