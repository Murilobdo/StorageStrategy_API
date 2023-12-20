using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;
using System.ComponentModel.Design;
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
            FinishCommandCommand finishCommand = new(commandId:1, PaymentEnum.Credit, companyId: 1);

            Assert.True(finishCommand.IsValid());
        }

        [Fact]
        public void Erro_ao_finalizar_uma_comanda_sem_commandId()
        {
            FinishCommandCommand finishCommand = new(commandId:0, payment:PaymentEnum.Credit, companyId: 1);

            Assert.True(MensagemDeErroExistente(finishCommand.GetErros(), "O id da comanda e obrigatório"));
        }

        [Fact]
        public void Erro_ao_finalizar_uma_comanda_sem_companyId()
        {
            FinishCommandCommand finishCommand = new(commandId:1, payment:PaymentEnum.Credit, companyId: 0);

            Assert.True(MensagemDeErroExistente(finishCommand.GetErros(), "O id da empresa e obrigatório"));
        }
    }
}
