using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;

namespace StorageStrategy.Tests.Faktory.command
{
    public class CreateCommandFactory : CommandFactory
    {
        private List<ProductEntity> _products;
        private List<EmployeeEntity> _employees;

        public CreateCommandFactory()
        {
        }

        protected override void AdicionarItens()
        {
            _products = new FakeProductRepository().products;
            for (int index = 0; index < _products.Count; index++)
            {
                items.Add(new CommandItemBase(
                    commandItemId: index + 1,
                    commandId: 0,
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
            _employees = new FakeEmployeeRepository().employees;

            command = new CreateCommandCommand(
                companyId: 1,
                name: "Gurinevers",
                employeeId: _employees[0].EmployeeId,
                discount: 0,
                increase:0,
                items: base.items,
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30)
            );
        }
    }
}
