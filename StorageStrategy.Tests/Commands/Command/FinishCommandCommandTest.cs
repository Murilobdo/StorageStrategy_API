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

        [Fact(DisplayName = "Sucesso ao Finalizar uma Comanda")]
        public void Sucesso_ao_finalizar_uma_comanda()
        {
            FinishCommandCommand finishCommand = new(
                commandId:1, 
                PaymentEnum.Credit, 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            Assert.True(finishCommand.IsValid());
        }

        [Fact(DisplayName = "Erro ao Finalizar uma Comanda sem Id")]
        public void Erro_ao_finalizar_uma_comanda_sem_commandId()
        {
            FinishCommandCommand finishCommand = new(
                commandId:0, 
                payment:PaymentEnum.Credit, 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            Assert.True(MensagemDeErroExistente(finishCommand.GetErros(), "O id da comanda e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Finalizar uma Comanda sem o Id da Empresa")]
        public void Erro_ao_finalizar_uma_comanda_sem_companyId()
        {
            FinishCommandCommand finishCommand = new(
                commandId:1,
                payment:PaymentEnum.Credit, 
                companyId: 0,
                discount: 0,
                increase: 0
            );

            Assert.True(MensagemDeErroExistente(finishCommand.GetErros(), "O id da empresa e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Finalizar uma Comanda com Desconto Negativo")]
        public void Erro_ao_finalizar_uma_comanda_com_desconto_negativo()
        {
            FinishCommandCommand finishCommand = new(
                commandId: 1,
                payment: PaymentEnum.Credit,
                companyId: 1,
                discount: -15,
                increase: 0
            );

            Assert.True(MensagemDeErroExistente(finishCommand.GetErros(), "O desconto não pode ser menor que zero"));
        }

        [Fact(DisplayName = "Erro ao Finalizar uma Comanda com Acréscimo Negativo")]
        public void Erro_ao_finalizar_uma_comanda_com_acrescimo_negativo()
        {
            FinishCommandCommand finishCommand = new(
                commandId: 1,
                payment: PaymentEnum.Credit,
                companyId: 1,
                discount: 0,
                increase: -15
            );

            Assert.True(MensagemDeErroExistente(finishCommand.GetErros(), "O acréscimo não pode ser menor que zero"));
        }
    }
}
