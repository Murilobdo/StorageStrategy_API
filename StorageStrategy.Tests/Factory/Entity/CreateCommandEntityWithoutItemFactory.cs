using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using StorageStrategy.Tests.Faktory.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Tests.Factory.Entity
{
    public class CreateCommandEntityWithoutItemFactory : CommandEntityFactory
    {
        protected override void AdicionarItens()
        {
        }

        protected override void CriarComanda()
        {
            command = new CommandEntity(
                    commandId: 2,
                    employeeId: 1,
                    name: "rodrigod",
                    totalCost: 5m,
                    totalPrice: 12m,
                    payment: null,
                    initialDate: DateTime.Now,
                    finalDate: null,
                    companyId: 1
                );
        }
    }
}
