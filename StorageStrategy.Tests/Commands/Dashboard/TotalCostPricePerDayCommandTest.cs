using StorageStrategy.Domain.Commands.Dashboard;
using Xunit;

namespace StorageStrategy.Tests.Commands.Dashboard
{
    public class TotalCostPricePerDayCommandTest : CommandBaseTest
    {
        [Fact]
        public void Sucesso_ao_criar_TotalCostPricePerDayCommand()
        {
            TotalCostPricePerDayCommand command = new(companyId:1, month:1, year:2024);

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_TotalCostPricePerDayCommand_sem_companyId()
        {
            TotalCostPricePerDayCommand command = new(companyId: 0, month: 1, year: 2024);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da Empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_TotalCostPricePerDayCommand_sem_month()
        {
            TotalCostPricePerDayCommand command = new(companyId: 1, month: 0, year: 2024);

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Mês e obrigatório"));
        }
    }
}
