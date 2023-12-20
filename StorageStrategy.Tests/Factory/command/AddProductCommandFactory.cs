
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Tests.FakeRepository;
using StorageStrategy.Tests.Faktory.command;

namespace StorageStrategy.Tests.Factory.command
{
    public class AddProductCommandWithOneItemFactory : CommandFactory
    {
        public AddProductCommandWithOneItemFactory() { }

        protected override void AdicionarItens()
        {
            
        }

        protected override void CriarComanda()
        {
            var commands = new FakeCommandRepository().commands;


        }
    }
}
