using MediatR;
using Moq;
using StorageStrategy.Domain.Commands.Category;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class CategoryHandlerTest
    {
        private Mock<IMediator> _mediator = new Mock<IMediator>();

        public CategoryHandlerTest()
        {
        }

        [Fact]
        public async Task Sucesso_ao_criar_uma_categoria() 
        {
            CreateCategoryCommand createCategory = new(1, "Sucesso", true, 1);

            var result = await _mediator.Object.Send(createCategory);

            Assert.Null(result);
        }
    }
}
