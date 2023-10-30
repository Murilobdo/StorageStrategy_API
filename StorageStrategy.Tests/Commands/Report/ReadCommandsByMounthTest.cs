using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Commands.Report;
using Xunit;

namespace StorageStrategy.Tests.Commands.Report
{
    public class ReadCommandsByMounthTest : CommandBaseTest
    {
        [Fact]
        public void Sucesso_ao_pesquisar_sem_funcionario()
        {
            ReadCommandsByMounthCommand search = new(1, 1, null);

            Assert.True(search.IsValid());
        }

        [Fact]
        public void Sucesso_ao_pesquisar_com_funcionario()
        {
            ReadCommandsByMounthCommand search = new(1, 1, 1);

            Assert.True(search.IsValid());
        }

        [Fact]
        public void Erro_ao_pesquisar_sem_mes()
        {
            ReadCommandsByMounthCommand search = new(1, 0, 1);

            Assert.True(MensagemDeErroExistente(search.GetErros(), "O Mês e obrigatório"));
        }

        [Fact]
        public void Erro_ao_pesquisar_sem_companyId()
        {
            ReadCommandsByMounthCommand search = new(0, 1, 1);

            Assert.True(MensagemDeErroExistente(search.GetErros(), "O Id da Empresa e obrigatório"));
        }


    }
}
