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
    public class CreateProductCommandTest : CommandBaseTest
    {
        public CreateProductCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_um_produto()
        {
            CreateProductCommand createProduct = new(1, "Sucesso Produto", 10, 20, 5, true, 1, 1);

            Assert.True(createProduct.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_um_produto_sem_nome()
        {
            CreateProductCommand createProduct = new(1, string.Empty, 10, 20, 5, true, 1, 1);

            Assert.True(MensagemDeErroExistente(createProduct.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_um_produto_sem_custo()
        {
            CreateProductCommand createProduct = new(1, "Sucesso Produto", 0, 20, 5, true, 1, 1);

            Assert.True(MensagemDeErroExistente(createProduct.GetErros(), "O valor de custo deve ser maior do que 0"));
        }

        [Fact]
        public void Erro_ao_criar_um_produto_sem_valor()
        {
            CreateProductCommand createProduct = new(1, "Sucesso Produto", 10, 0, 5, true, 1, 1);

            Assert.True(MensagemDeErroExistente(createProduct.GetErros(), "O valor de venda deve ser maior do que 0"));
        }

        [Fact]
        public void Erro_ao_criar_um_produto_sem_quantidade()
        {
            CreateProductCommand createProduct = new(1, "Sucesso Produto", 10, 20, 0, true, 1, 1);

            Assert.True(MensagemDeErroExistente(createProduct.GetErros(), "A quantidade deve ser maior do que 0"));
        }

        [Fact]
        public void Erro_ao_criar_um_produto_sem_categoryId()
        {
            CreateProductCommand createProduct = new(1, "Sucesso Produto", 10, 20, 5, true, 0, 1);

            Assert.True(MensagemDeErroExistente(createProduct.GetErros(), "O id da categoria e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_um_produto_sem_companyId()
        {
            CreateProductCommand createProduct = new(1, "Sucesso Produto", 10, 20, 5, true, 1, 0);

            Assert.True(MensagemDeErroExistente(createProduct.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
