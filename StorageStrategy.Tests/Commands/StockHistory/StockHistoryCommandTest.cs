
using StorageStrategy.Domain.Commands.StockHistory;
using StorageStrategy.Models;
using Xunit;

namespace StorageStrategy.Tests.Commands.StockHistory
{
    public class StockHistoryCommandTest : CommandBaseTest
    {
        private readonly CreateStockHistoryCommand _command;

        public StockHistoryCommandTest()
        {
            _command = new CreateStockHistoryCommand
            {
                CompanyId = 1,
                Products = new List<StockHistoryItemEntity>
                {
                    new StockHistoryItemEntity
                    {
                        ProductId = 1,
                        Quantity = 10,
                        Taxing = 0.1m
                    }
                }
            };
        }

        [Fact(DisplayName = "Sucesso ao Abastecer Estoque")]
        public void Sucesso_ao_abastecer_estoque()
        {
            Assert.True(_command.IsValid());
        }

        [Fact(DisplayName = "Erro ao Tentar Abastecer o Estoque sem CompanyId")]
        public void Erro_ao_tentar_abastecer_sem_companyId()
        {
            _command.CompanyId = 0;
            Assert.True(MensagemDeErroExistente(_command.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Tentar Abastecer o Estoque sem Produtos")]
        public void Erro_ao_tentar_abastecer_sem_produtos()
        {
            _command.Products = new List<StockHistoryItemEntity>();
            Assert.True(MensagemDeErroExistente(_command.GetErros(), "Selecione pelo menos um produto"));
        }
    }
}
