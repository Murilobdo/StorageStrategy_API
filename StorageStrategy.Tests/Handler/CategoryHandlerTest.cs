using AutoMapper;
using MediatR;
using Moq;
using StorageStrategy.Domain.AutoMapper;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Handlers;
using StorageStrategy.Tests.FakeRepository;
using System.Threading;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class CategoryHandlerTest : HandlerBaseTest
    {
        private CategoryHandler _handler;
        private FakeCategoryRepository _repo;
        private IMapper _mapper;
        private CancellationToken _cancellationToken;

        public CategoryHandlerTest()
        {
            _cancellationToken = new CancellationToken();
            _mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new CategoryProfile());
            }).CreateMapper();

            _repo = new FakeCategoryRepository();

            _handler = new CategoryHandler(
                    _repo,
                    _mapper
                );
        }

        #region CREATE CATEGORY
        [Fact(DisplayName = "Sucesso ao Criar uma Categoria")]
        public async Task Sucesso_ao_criar_uma_categoria()
        {
            CreateCategoryCommand category = new(1, "Sucesso", true, 1);

            var result = await _handler.Handle(category, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro ao Criar uma Categoria com Nome ja Existente")]
        public async Task Sucesso_ao_criar_uma_categoria_com_nome_ja_existente()
        {
            CreateCategoryCommand category = new(1, "Sucesso", true, 1);

            category.Name = _repo.categorys[0].Name;

            var result = await _handler.Handle(category, _cancellationToken);

            Assert.False(IsValid(result, "Ja existe uma categoria com esse nome"));
        }
        #endregion

        #region UPDATE CATEGORY
        [Fact(DisplayName = "Sucesso ao Atualizar uma Categoria")]
        public async Task Sucesso_ao_atualizar_uma_categoria()
        {
            UpdateCategoryCommand category = new(1, "Sucesso", true, 1);

            var result = await _handler.Handle(category, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro ao Atualizar uma Categoria Inexistente")]
        public async Task Erro_ao_atualizar_uma_categoria_inexistente()
        {
            UpdateCategoryCommand category = new(31, "Sucesso", true, 1);

            var result = await _handler.Handle(category, _cancellationToken);

            Assert.False(IsValid(result, "Categoria não encontrada para edição"));
        }
        #endregion

        #region DELETE CATEGORY
        [Fact(DisplayName = "Sucesso ao Deletar uma Categoria")]
        public async Task Sucesso_ao_deletar_uma_categoria()
        {
            DeleteCategoryCommand category = new(1, 1);

            var result = await _handler.Handle(category, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro ao Deletar uma Categoria Inexistente")]
        public async Task Erro_ao_deletar_uma_categoria()
        {
            DeleteCategoryCommand category = new(31, 1);

            var result = await _handler.Handle(category, _cancellationToken);

            Assert.False(IsValid(result, "Categoria não encontrada para exclusão"));
        }
        #endregion
    }
}
