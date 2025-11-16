using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using StorageStrategy.Domain.AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Handlers;
using StorageStrategy.Domain.Handlers.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using StorageStrategy.Tests.Faktory.command;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class CommandHandlerTest : HandlerBaseTest
    {
        private FakeProductRepository _repoProduct;
        private FakeCommandRepository _repoCommand;
        private FakeEmployeeRepository _repoEmployee;
        private IClientRepository _repoClient;
        private IMapper _mapper;
        private CancellationToken _cancellationToken;
        private ILoggerFactory _log;

        public CommandHandlerTest()
        {
            _cancellationToken = new CancellationToken();
            
            _repoProduct = new FakeProductRepository();
            _repoEmployee = new FakeEmployeeRepository();
            _repoCommand = new FakeCommandRepository();
            _log = new Mock<ILoggerFactory>().Object;

            MapperConfigurationExpression cfg = new MapperConfigurationExpression();
            cfg.AddProfile(new CommandProfile());
            _mapper = new MapperConfiguration(cfg, _log)
                .CreateMapper();

           

        }

        #region CREATE COMMAND
        [Fact(DisplayName = "Sucesso ao Criar uma Comanda com Itens")]
        public async Task Sucesso_ao_criar_uma_comanda_com_itens()
        {
            var handler = new CreateCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            CreateCommandCommand command = (CreateCommandCommand) new CreateCommandFactory().command;

            var result = await handler.Handle(command, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Sucesso ao Comparar o Preco Total dos Itens com o Preco Total da Comanda cadastrada")]
        public async Task Suceso_ao_comparar_o_preco_total_dos_itens_com_o_preco_total_da_comanda_cadastrada()
        { 
            var handler = new CreateCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            CreateCommandCommand command = (CreateCommandCommand) new CreateCommandFactory().command;
            var totalPrice = command.Items.Sum(p => p.Price);

            var result = await handler.Handle(command, _cancellationToken);
            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.TotalPrice == totalPrice);
        }

        [Fact(DisplayName = "Sucesso ao Comparar o Custo Total dos Itens com o Custo Total da Comanda cadastrada")]
        public async Task Suceso_ao_comparar_o_custo_total_dos_itens_com_o_preco_total_da_comanda_cadastrada()
        {
            var handler = new CreateCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            CreateCommandCommand command = (CreateCommandCommand)new CreateCommandFactory().command;
            var totalCost = command.Items.Sum(p => p.Cost);

            var result = await handler.Handle(command, _cancellationToken);
            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.TotalCost == totalCost);
        }

        [Fact(DisplayName = "Erro ao Criar uma Comanda Sem Funcionario")]
        public async Task Erro_ao_criar_uma_comanda_sem_funcionario()
        {
            var handler = new CreateCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            CreateCommandCommand command = (CreateCommandCommand)new CreateCommandFactory().command;

            command.EmployeeId = 999;

            var result = await handler.Handle(command, _cancellationToken);

            Assert.False(IsValid(result, "Funcionario não encontrado"));
        }

        [Fact(DisplayName = "Erro ao Criar uma Comanda Sem Produtos Disponiveis em Estoque")]
        public async Task Erro_ao_criar_uma_comanda_sem_produtos_disponiveis_em_estoque()
        {
            var handler = new CreateCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            CreateCommandCommand command = (CreateCommandCommand)new CreateCommandFactory().command;

            command.Items[0].Qtd = 80;

            var result = await handler.Handle(command, _cancellationToken);

            Assert.False(IsValid(result, $"Quantidade indisponivel em estoque [{command.Items[0].Name.Trim()}]"));
        }
        #endregion

        #region FINISH COMMAND
        [Fact(DisplayName = "Sucesso ao Finalizar uma Comanda")]
        public async Task Sucesso_ao_finalizar_uma_comanda()
        {
            var handler = new FinishCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            FinishCommandCommand command = new(
                commandId: _repoCommand.commands[0].CommandId, 
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30), 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            var result = await handler.Handle(command, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Sucesso ao Adicionar o Metodo de Pagamento")]
        public async Task Sucesso_ao_adicionar_o_metodo_de_pagamento()
        {
            var handler = new FinishCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);

            var paymentAdd = new PaymentCommand(1, PaymentEnum.Cash, 30);

            FinishCommandCommand command = new(
                commandId: _repoCommand.commands[0].CommandId, 
                payment: paymentAdd, 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            var result = await handler.Handle(command, _cancellationToken);

            var paymentHasAddWithSuccess = command.Payments.Any(x => x.Method == paymentAdd.Method && x.Amount == paymentAdd.Amount);
            Assert.True(paymentHasAddWithSuccess);
        }

        [Fact(DisplayName = "Sucesso ao validar a Data do Pagamento da Comanda")]
        public async Task Sucesso_ao_validar_a_data_do_pagamento_da_comanda()
        {
            var handler = new FinishCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            FinishCommandCommand command = new(
                commandId: _repoCommand.commands[0].CommandId, 
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30), 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            var result = await handler.Handle(command, _cancellationToken);
            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.FinalDate.HasValue);
        }

        [Fact(DisplayName = "Erro ao Finalizar uma Comanda Inexistente")]
        public async Task Erro_ao_finalizar_uma_comanda_inexistente()
        {
            var handler = new FinishCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            FinishCommandCommand command = new(
                commandId: 80, 
                payment: new PaymentCommand(1, PaymentEnum.Cash, 30), 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            var result = await handler.Handle(command, _cancellationToken);

            Assert.False(IsValid(result, "Comanda não encontrada"));
        }
        #endregion

        #region ADD PRODUCT COMMAND
        [Fact(DisplayName = "Sucesso ao Adicionar Produtos na Comanda")]
        public async Task Sucesso_ao_adicionar_produtos_na_comanda()
        {
            var handler = new AddProductCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            var databaseCommand = new CreateCommandFactory().command;
            var itemsToAdd = databaseCommand.Items.Take(1).ToList();
            
            itemsToAdd[0].CommandItemId = 0;
            //comanda de id 2 e uma comanda sem itens
            AddProductCommandCommand command = new (
                commandId: 2, 
                companyId: 1, 
                items: itemsToAdd
            );

            var result = await handler.Handle(command, _cancellationToken);

            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.Items.Any(p => p.Name == itemsToAdd[0].Name));
        }

        [Fact(DisplayName = "Sucesso ao Remover Produtos da Comanda")]
        public async Task Sucesso_ao_remover_produtos_da_comanda()
        {
            var handler = new AddProductCommandHandler(_repoProduct, _repoCommand, _repoEmployee, _mapper, _repoClient);
            
            var databaseCommand = new CreateCommandFactory().command;
            var itemsToAdd = databaseCommand.Items.Take(1).ToList();

            //comanda de id 1 e uma comanda com 4 itens
            AddProductCommandCommand command = new(
                commandId: 1,
                companyId: 1,
                items: itemsToAdd
            );

            var result = await handler.Handle(command, _cancellationToken);

            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.Items.Count == 1);
        }
        #endregion
    }
}
