using StorageStrategy.Domain.Commands.PixCharge;
using Xunit;

namespace StorageStrategy.Tests.Commands.PixCharge
{
    public class CreatePixChargeCommandTest : CommandBaseTest
    {
        [Fact]
        public void Sucesso_ao_validar_criacao_de_cobranca_pix()
        {
            CreatePixChargeCommand command = new() { CommandId = 1, CompanyId = 1 };

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_cobranca_pix_sem_comanda()
        {
            CreatePixChargeCommand command = new() { CommandId = 0, CompanyId = 1 };

            Assert.True(MensagemDeErroExistente(command.GetErros(), "A comanda é obrigatória"));
        }

        [Fact]
        public void Erro_ao_criar_cobranca_pix_sem_empresa()
        {
            CreatePixChargeCommand command = new() { CommandId = 1, CompanyId = 0 };

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da empresa é obrigatório"));
        }
    }
}
