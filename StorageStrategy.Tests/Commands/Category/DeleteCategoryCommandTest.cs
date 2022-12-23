﻿using StorageStrategy.Domain.Commands;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Models;
using System.Linq;
using Xunit;

namespace StorageStrategy.Tests.Commands.Category
{
    public class DeleteCategoryCommandTest : CommandBaseTest
    {

        public DeleteCategoryCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_excluir_uma_categoria()
        {
            DeleteCategoryCommand createCategory = new(categoryId: 1, companyId: 1);

            Assert.True(createCategory.IsValid());
        }

        [Fact]
        public void Erro_ao_excluir_uma_categoria_sem_id()
        {
            DeleteCategoryCommand createCategory = new(categoryId: 0, companyId: 1);

            Assert.True(MensagemDeErroExistente(createCategory.GetErros(), "O Id e obrigatório"));
        }

        [Fact]
        public void Erro_ao_excluir_uma_categoria_sem_id_empresa()
        {
            DeleteCategoryCommand createCategory = new(categoryId: 1, companyId: 0);

            Assert.True(MensagemDeErroExistente(createCategory.GetErros(), "O Id da empresa e obrigatório"));
        }

    }
}
