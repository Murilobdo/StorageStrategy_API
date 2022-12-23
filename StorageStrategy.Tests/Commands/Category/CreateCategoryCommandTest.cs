using StorageStrategy.Domain.Commands;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Models;
using System.Linq;
using Xunit;

namespace StorageStrategy.Tests.Commands.Category
{
    public class CreateCategoryCommandTest : CommandBaseTest
    {

        public CreateCategoryCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_uma_categoria()
        {
            CreateCategoryCommand createCategory = new(1, "Sucesso", true, 1);

            Assert.True(createCategory.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_uma_categoria_sem_nome()
        {
            CreateCategoryCommand createCategory = new(1, string.Empty, true, 1);

            Assert.True(MensagemDeErroExistente(createCategory.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_categoria_sem_o_id_empresa()
        {
            CreateCategoryCommand createCategory = new(1, "test", true, 0);

            Assert.True(MensagemDeErroExistente(createCategory.GetErros(), "O Id da empresa e obrigatório"));
        }
    }
}
