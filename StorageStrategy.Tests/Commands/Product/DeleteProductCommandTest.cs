using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StorageStrategy.Tests.Commands.Product
{
    public class DeleteProductCommandTest : CommandBaseTest
    {
        public DeleteProductCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_excluir_um_produto()
        {
            DeleteProductCommand updateProduct = new(1, 1);

            Assert.True(updateProduct.IsValid());
        }

        [Fact]
        public void Erro_ao_excluir_um_produto_sem_id()
        {
            DeleteProductCommand updateProduct = new(0, 1);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "O Id e obrigatório"));
        }


        [Fact]
        public void Erro_ao_excluir_um_produto_sem_companyId()
        {
            DeleteProductCommand updateProduct = new(1, 0);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
