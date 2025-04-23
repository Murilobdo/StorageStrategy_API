using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;
using Xunit;

namespace StorageStrategy.Tests.Commands.Coommand
{
    public class CreateCommandCommandTest : CommandBaseTest
    {
        public CreateCommandCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_uma_comanda()
        {
            CreateCommandCommand createCommand = new(
                companyId:1, 
                name:"Comanda Teste", 
                employeeId:1, 
                discount: 0,
                increase:0,
                items:GetCommandItems(), 
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30)
            );

            Assert.True(createCommand.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_uma_comanda_sem_companyId()
        {
            CreateCommandCommand createCommand = new(
                companyId: 0,
                name: "Comanda Teste",
                employeeId: 1,
                discount: 0,
                increase: 0,
                items: GetCommandItems(),
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30)
            );

            Assert.True(MensagemDeErroExistente(createCommand.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_comanda_sem_employeeId()
        {
            CreateCommandCommand createCommand = new(
                companyId: 1,
                name: "Comanda Teste",
                employeeId: 0,
                discount: 0,
                increase: 0,
                items: GetCommandItems(),
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30)
            );

            Assert.True(MensagemDeErroExistente(createCommand.GetErros(), "O funcionário e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_comanda_sem_itens()
        {
            CreateCommandCommand createCommand = new(
                companyId: 1,
                name: "Comanda Teste",
                employeeId: 1,
                discount: 0,
                increase: 0,
                items: new List<CommandItemBase>(),
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30)
            );

            Assert.True(MensagemDeErroExistente(createCommand.GetErros(), "Essa comanda não possui produtos"));
        }

        [Fact]
        public void Erro_ao_criar_uma_comanda_com_desconto_negativo()
        {
            CreateCommandCommand createCommand = new(
                companyId: 1,
                name: "Comanda Teste",
                employeeId: 1,
                discount: -30,
                increase: 0,
                items: GetCommandItems(),
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30)
            );

            Assert.True(MensagemDeErroExistente(createCommand.GetErros(), "O desconto não pode ser negativo"));
        }

        [Fact]
        public void Erro_ao_criar_uma_comanda_com_acrescimo_negativo()
        {
            CreateCommandCommand createCommand = new(
                companyId: 1,
                name: "Comanda Teste",
                employeeId: 1,
                discount: 0,
                increase: -30,
                items: GetCommandItems(),
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30)
            );

            Assert.True(MensagemDeErroExistente(createCommand.GetErros(), "O acréscimo não pode ser negativo"));
        }

        private List<CommandItemBase> GetCommandItems() 
        {
            return new List<CommandItemBase>()
            {
                new CommandItemBase
                {
                    CommandId = 1,
                }
            };
        }

    }
}
