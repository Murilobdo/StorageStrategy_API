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
            CreateExpenseCommand command = new(1, "Description", 1, DateTime.Now, 10m);

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_uma_despesa_companyId()
        {
            CreateExpenseCommand command = new(0, "Description", 1, DateTime.Now, 10m);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_despesa_descricao()
        {
            CreateExpenseCommand command = new(1, string.Empty, 1, DateTime.Now, 10m);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "A Descrição e obrigatória"));
        }

        [Fact]
        public void Erro_ao_criar_uma_despesa_categoria()
        {
            CreateExpenseCommand command = new(1, "Description", 0, DateTime.Now, 10m);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "A caregoria da despesa e obrigatória"));
        }

        [Fact]
        public void Erro_ao_criar_uma_despesa_sem_valor()
        {
            CreateExpenseCommand command = new(1, "Description", 1, DateTime.Now, 0);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Valor da despesa deve ser maior do que 0"));
        }
    }
}
