using StorageStrategy.Domain.Commands.Expenses;
using Xunit;

namespace StorageStrategy.Tests.Commands.Extenses
{
    public class DeleteExpensesCommandTest : CommandBaseTest
    {
        public DeleteExpensesCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_excluir_uma_despesa()
        {
            DeleteExpenseCommand command = new(1, 1);

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_excluir_uma_despesa_sem_id()
        {
            DeleteExpenseCommand command = new(0, 1);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id e obrigatório"));
        }

        [Fact]
        public void Erro_ao_excluir_uma_despesa_sem_companyId()
        {
            DeleteExpenseCommand command = new(1, 0);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
