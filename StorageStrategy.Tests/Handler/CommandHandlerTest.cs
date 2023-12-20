using AutoMapper;
using StorageStrategy.Domain.AutoMapper;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Handlers;
using StorageStrategy.Tests.FakeRepository;
using StorageStrategy.Tests.Faktory;
using StorageStrategy.Tests.Faktory.command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class CommandHandlerTest : HandlerBaseTest
    {
        private CommandHandler _handler;
        private FakeProductRepository _repoProduct;
        private FakeCommandRepository _repoCommand;
        private FakeEmployeeRepository _repoEmployee;
        private IMapper _mapper;
        private CancellationToken _cancellationToken;

        public CommandHandlerTest()
        {
            _cancellationToken = new CancellationToken();
            
            _repoProduct = new FakeProductRepository();
            _repoEmployee = new FakeEmployeeRepository();
            _repoCommand = new FakeCommandRepository(_repoProduct);

            _mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new CommandProfile());
            }).CreateMapper();

            _handler = new CommandHandler(
                    _repoProduct,
                    _repoCommand,
                    _repoEmployee,
                    _mapper
                );
        }

        [Fact(DisplayName = "Sucesso ao Criar uma Comanda com Itens")]
        public async Task Sucesso_ao_criar_uma_comanda_com_itens()
        {
            CreateCommandCommand command = (CreateCommandCommand) new CreateCommandFactory().command;

            var result = await _handler.Handle(command, _cancellationToken);

            Assert.True(result.Success);
        }
    }
}
