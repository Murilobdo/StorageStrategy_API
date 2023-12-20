using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Tests.Faktory.command
{
    public abstract class CommandFactory
    {
        public CommandCommandBase command;
        public List<CommandItemBase> items;

        public CommandFactory()
        {
            CriarComanda();
            AdicionarItens();
        }

        protected abstract void CriarComanda();

        protected abstract void AdicionarItens();
    }
}
