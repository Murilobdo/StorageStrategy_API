using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Tests.Faktory.Entity
{
    public abstract class CommandEntityFactory
    {
        public CommandEntity command;
        public List<CommandItemEntity> items;

        public CommandEntityFactory()
        {
            CriarComanda();
            AdicionarItens();
        }

        protected abstract void CriarComanda();
        protected abstract void AdicionarItens();
    }
}
