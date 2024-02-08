using AutoMapper;
using StorageStrategy.Domain.AutoMapper;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Handlers;
using StorageStrategy.Tests.FakeRepository;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class ProductHandlerTest : HandlerBaseTest
    {
        public ProductHandler _handler;
        public IMapper _mapper;
        public CancellationToken _cancellationToken;
        public FakeCategoryRepository _repoCategory;
        public FakeProductRepository _repo;
        public FakeEmployeeRepository _repoEmployee;

        public ProductHandlerTest()
        {
            _repo = new FakeProductRepository();
            _repoEmployee = new FakeEmployeeRepository();
            _repoCategory = new FakeCategoryRepository();

            _mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new ProductProfile());
            }).CreateMapper();

            _handler = new ProductHandler(
                    _repo,
                    _repoCategory,
                    _mapper
                );
        }

        #region CREATE PRODUCT
        [Fact(DisplayName = "Sucesso Ao Criar Um Produto")]
        public async Task Sucesso_ao_criar_um_produto()
        {
            CreateProductCommand create = _repo.CreateProductCommand;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro Ao Criar Um Produto Com Nome Existente")]
        public async Task Erro_ao_criar_um_produto_com_nome_existente()
        {
            CreateProductCommand create = _repo.CreateProductCommand;

            create.Name = _repo.products[0].Name;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.False(IsValid(result, "Ja existe um produto com esse nome"));
        }

        [Fact(DisplayName = "Erro Ao Criar Um Produto Sem Categoria")]
        public async Task Erro_ao_criar_um_produto_sem_cateogira()
        {
            CreateProductCommand create = _repo.CreateProductCommand;

            create.CategoryId = 58;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.False(IsValid(result, "Categoria não encontrada"));
        }
        #endregion

        #region UPDATE PRODUCT
        [Fact(DisplayName = "Sucesso Ao Atualizar Um Produto")]
        public async Task Sucesso_ao_atualizar_um_produto()
        {
            UpdateProductCommand update = _repo.UpdateProductCommand;

            var result = await _handler.Handle(update, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro Ao Atualizar Um Produto Inexistente")]
        public async Task Erro_ao_atualizar_um_produto_inexistente()
        {
            UpdateProductCommand update = _repo.UpdateProductCommand;

            update.ProductId = 15;
            update.CompanyId = 15;

            var result = await _handler.Handle(update, _cancellationToken);

            Assert.False(IsValid(result, "Produto não encontrado para a atualização"));
        }

        [Fact(DisplayName = "Erro Ao Atualizar Um Produto Com Nome Existente")]
        public async Task Erro_ao_atualizar_um_produto_com_nome_existente()
        {
            UpdateProductCommand update = _repo.UpdateProductCommand;

            update.Name = _repo.products[1].Name;

            var result = await _handler.Handle(update, _cancellationToken);

            Assert.False(IsValid(result, "Ja existe um produto com esse nome"));
        }

        [Fact(DisplayName = "Erro Ao Atualizar Um Produto Com Categoria Inexistente")]
        public async Task Erro_ao_atualizar_um_produto_com_categoria_inexistente()
        {
            UpdateProductCommand update = _repo.UpdateProductCommand;

            update.CategoryId = 58;

            var result = await _handler.Handle(update, _cancellationToken);

            Assert.False(IsValid(result, "Categoria não encontrada"));
        }
        #endregion 
    }
}
