using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace StorageStrategy.Tests.Faktory.Entity
{
    public class CreateCommandEntityWithItemsFactory : CommandEntityFactory
    {
        private List<ProductEntity> _products;

        public CreateCommandEntityWithItemsFactory()
        {
        }

        protected override void AdicionarItens()
        {
            _products = new FakeProductRepository().products;

            for (int index = 0; index < _products.Count; index++)
            {
                command.Items.Add(new CommandItemEntity(
                    commandItemId: index + 1,
                    commandId: command.CommandId,
                    productId: _products[index].ProductId,
                    name: _products[index].Name,
                    cost: _products[index].Cost,
                    price: _products[index].Price,
                    qtd: 1,
                    taxing: _products[index].Taxing
                ));
            }
        }

        protected override void CriarComanda()
        {
            command = new CommandEntity(commandId: 1, employeeId: 1, name: "Gurinevers", totalCost: 62.5M, totalPrice: 181, payment: PaymentEnum.Pix, initialDate: DateTime.Now, finalDate: DateTime.Now.AddDays(1), companyId: 1);
        }
    }
}
