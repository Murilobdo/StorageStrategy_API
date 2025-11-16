using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StorageStrategy.Domain.AutoMapper;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Handlers;
using StorageStrategy.Domain.Handlers.Product;
using StorageStrategy.Domain.Services.MinioStorage;
using StorageStrategy.Tests.FakeRepository;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class ProductHandlerTest : HandlerBaseTest
    {
        public IMapper _mapper;
        public CancellationToken _cancellationToken;
        public FakeCategoryRepository _repoCategory;
        public FakeProductRepository _repoProduct;
        public FakeEmployeeRepository _repoEmployee;
        public IStorageFile _storage;
        private ILoggerFactory _log;

        public ProductHandlerTest()
        {
            _repoProduct = new FakeProductRepository();
            _repoEmployee = new FakeEmployeeRepository();
            _repoCategory = new FakeCategoryRepository();
            _log = new Mock<ILoggerFactory>().Object;

            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile(new ProductProfile());
            _mapper = new MapperConfiguration(cfg).CreateMapper();
        }

        #region CREATE PRODUCT
        [Fact(DisplayName = "Sucesso Ao Criar Um Produto")]
        public async Task Sucesso_ao_criar_um_produto()
        {
            var _handler = new CreateProductHandle(_repoProduct, _repoCategory, _mapper, _storage);
            
            CreateProductCommand create = _repoProduct.CreateProductCommand;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro Ao Criar Um Produto Com Nome Existente")]
        public async Task Erro_ao_criar_um_produto_com_nome_existente()
        {
            var _handler = new CreateProductHandle(_repoProduct, _repoCategory, _mapper, _storage);
            
            CreateProductCommand create = _repoProduct.CreateProductCommand;

            create.Name = _repoProduct.products[0].Name;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.False(IsValid(result, "Ja existe um produto com esse nome"));
        }

        [Fact(DisplayName = "Erro Ao Criar Um Produto Sem Categoria")]
        public async Task Erro_ao_criar_um_produto_sem_cateogira()
        {
            var _handler = new CreateProductHandle(_repoProduct, _repoCategory, _mapper, _storage);
            
            CreateProductCommand create = _repoProduct.CreateProductCommand;

            create.CategoryId = 58;

            var result = await _handler.Handle(create, _cancellationToken);

            Assert.False(IsValid(result, "Categoria não encontrada"));
        }
        #endregion

        #region UPDATE PRODUCT
        [Fact(DisplayName = "Sucesso Ao Atualizar Um Produto")]
        public async Task Sucesso_ao_atualizar_um_produto()
        {
            var _handler = new UpdateProductHandle(_repoProduct, _repoCategory, _mapper);
            
            UpdateProductCommand update = _repoProduct.UpdateProductCommand;

            var result = await _handler.Handle(update, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro Ao Atualizar Um Produto Inexistente")]
        public async Task Erro_ao_atualizar_um_produto_inexistente()
        {
            var _handler = new UpdateProductHandle(_repoProduct, _repoCategory, _mapper);
            
            UpdateProductCommand update = _repoProduct.UpdateProductCommand;

            update.ProductId = 15;
            update.CompanyId = 15;

            var result = await _handler.Handle(update, _cancellationToken);

            Assert.False(IsValid(result, "Produto não encontrado para a atualização"));
        }

        [Fact(DisplayName = "Erro Ao Atualizar Um Produto Com Nome Existente")]
        public async Task Erro_ao_atualizar_um_produto_com_nome_existente()
        {
            var _handler = new UpdateProductHandle(_repoProduct, _repoCategory, _mapper);
            
            UpdateProductCommand update = _repoProduct.UpdateProductCommand;

            update.Name = _repoProduct.products[1].Name;

            var result = await _handler.Handle(update, _cancellationToken);

            Assert.False(IsValid(result, "Ja existe um produto com esse nome"));
        }

        [Fact(DisplayName = "Erro Ao Atualizar Um Produto Com Categoria Inexistente")]
        public async Task Erro_ao_atualizar_um_produto_com_categoria_inexistente()
        {
            var _handler = new UpdateProductHandle(_repoProduct, _repoCategory, _mapper);
            
            UpdateProductCommand update = _repoProduct.UpdateProductCommand;

            update.CategoryId = 58;

            var result = await _handler.Handle(update, _cancellationToken);

            Assert.False(IsValid(result, "Categoria não encontrada"));
        }
        #endregion 
    }
}
