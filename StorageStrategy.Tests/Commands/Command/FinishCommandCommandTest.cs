using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;
using Xunit;

namespace StorageStrategy.Tests.Commands.Coommand
{
    public class FinishCommandCommandTest : CommandBaseTest
    {
        public FinishCommandCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_finalizar_uma_comanda()
        {
            FinishCommandCommand finishCommand = new(1, PaymentEnum.Credit);

            Assert.True(finishCommand.IsValid());
        }

        [Fact]
        public void Erro_ao_finalizar_uma_comanda_sem_commandId()
        {
            FinishCommandCommand finishCommand = new(0, PaymentEnum.Credit);

            Assert.True(MensagemDeErroExistente(finishCommand.GetErros(), "O id da comanda e obrigatório"));
        }
    }
}
