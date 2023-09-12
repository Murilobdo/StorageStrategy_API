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
    public class UpdateProductCommandTest : CommandBaseTest
    {
        public UpdateProductCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_atualizar_um_produto()
        {
            UpdateProductCommand updateProduct = new(1, "Sucesso Produto", 10, 20, 5, 10, true, 1,  1, 0);

            Assert.True(updateProduct.IsValid());
        }

        [Fact]
        public void Erro_ao_atualizar_um_produto_sem_id()
        {
            UpdateProductCommand updateProduct = new(0, "Sucesso produto", 10, 20, 5, 10, true, 1,  1, 0);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "O Id e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_produto_sem_nome()
        {
            UpdateProductCommand updateProduct = new(1, string.Empty, 10, 20, 5, 10, true, 1,  1, 0);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_produto_sem_custo()
        {
            UpdateProductCommand updateProduct = new(1, "Sucesso Produto", 0, 20, 5, 10, true, 1,  1, 0);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "O valor de custo deve ser maior do que 0"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_produto_sem_valor()
        {
            UpdateProductCommand updateProduct = new(1, "Sucesso Produto", 10, 0, 5, 10, true, 1,  1, 0);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "O valor de venda deve ser maior do que 0"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_produto_sem_quantidade()
        {
            UpdateProductCommand updateProduct = new(1, "Sucesso Produto", 10, 20, 0, 10, true, 1,  1, 0);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "A quantidade deve ser maior do que 0"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_produto_sem_categoryId()
        {
            UpdateProductCommand updateProduct = new(1, "Sucesso Produto", 10, 20, 5, 10, true, 0,  1, 0);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "O id da categoria e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_produto_sem_companyId()
        {
            UpdateProductCommand updateProduct = new(1, "Sucesso Produto", 10, 20, 5, 10, true, 1, 1, 0);

            Assert.True(MensagemDeErroExistente(updateProduct.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_um_produto_sem_taxing()
        {
            CreateProductCommand createProduct = new(1, "Sucesso Produto", 10, 20, 5, 10, true, 1, 1, -1);

            Assert.True(MensagemDeErroExistente(createProduct.GetErros(), "O imposto deve ser pelo menos 0"));
        }
    }
}
