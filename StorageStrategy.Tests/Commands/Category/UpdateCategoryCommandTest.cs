using StorageStrategy.Domain.Commands;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Models;
using System.Linq;
using Xunit;

namespace StorageStrategy.Tests.Commands.Category
{
    public class UpdateCategoryCommandTest : CommandBaseTest
    {

        public UpdateCategoryCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_atualizar_uma_categoria()
        {
            UpdateCategoryCommand createCategory = new(1, "Sucesso", true, 1);

            Assert.True(createCategory.IsValid());
        }

        [Fact]
        public void Erro_ao_atualizar_uma_categoria_sem_nome()
        {
            UpdateCategoryCommand createCategory = new(1, string.Empty, true, 1);

            Assert.True(MensagemDeErroExistente(createCategory.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_uma_categoria_sem_o_id()
        {
            UpdateCategoryCommand createCategory = new(0, "test", true, 1);

            Assert.True(MensagemDeErroExistente(createCategory.GetErros(), "O Id e obrigatório"));
        }

        [Fact]
        public void Erro_ao_atualizar_uma_categoria_sem_o_id_empresa()
        {
            UpdateCategoryCommand createCategory = new(1, "test", true, 0);

            Assert.True(MensagemDeErroExistente(createCategory.GetErros(), "O Id da Empresa e obrigatório"));
        }
    }
}
