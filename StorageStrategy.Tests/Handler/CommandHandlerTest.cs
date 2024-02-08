using AutoMapper;
using StorageStrategy.Domain.AutoMapper;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Handlers;
using StorageStrategy.Models;
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
            _repoCommand = new FakeCommandRepository();

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

        #region CREATE COMMAND
        [Fact(DisplayName = "Sucesso ao Criar uma Comanda com Itens")]
        public async Task Sucesso_ao_criar_uma_comanda_com_itens()
        {
            CreateCommandCommand command = (CreateCommandCommand) new CreateCommandFactory().command;

            var result = await _handler.Handle(command, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Sucesso ao Comparar o Preco Total dos Itens com o Preco Total da Comanda cadastrada")]
        public async Task Suceso_ao_comparar_o_preco_total_dos_itens_com_o_preco_total_da_comanda_cadastrada()
        { 
            CreateCommandCommand command = (CreateCommandCommand) new CreateCommandFactory().command;
            var totalPrice = command.Items.Sum(p => p.Price);

            var result = await _handler.Handle(command, _cancellationToken);
            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.TotalPrice == totalPrice);
        }

        [Fact(DisplayName = "Sucesso ao Comparar o Custo Total dos Itens com o Custo Total da Comanda cadastrada")]
        public async Task Suceso_ao_comparar_o_custo_total_dos_itens_com_o_preco_total_da_comanda_cadastrada()
        {
            CreateCommandCommand command = (CreateCommandCommand)new CreateCommandFactory().command;
            var totalCost = command.Items.Sum(p => p.Cost);

            var result = await _handler.Handle(command, _cancellationToken);
            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.TotalCost == totalCost);
        }

        [Fact(DisplayName = "Erro ao Criar uma Comanda Sem Funcionario")]
        public async Task Erro_ao_criar_uma_comanda_sem_funcionario()
        {
            CreateCommandCommand command = (CreateCommandCommand)new CreateCommandFactory().command;

            command.EmployeeId = 999;

            var result = await _handler.Handle(command, _cancellationToken);

            Assert.False(IsValid(result, "Funcionario não encontrado"));
        }

        [Fact(DisplayName = "Erro ao Criar uma Comanda Sem Produtos Disponiveis em Estoque")]
        public async Task Erro_ao_criar_uma_comanda_sem_produtos_disponiveis_em_estoque()
        {
            CreateCommandCommand command = (CreateCommandCommand)new CreateCommandFactory().command;

            command.Items[0].Qtd = 80;

            var result = await _handler.Handle(command, _cancellationToken);

            Assert.False(IsValid(result, $"Quantidade indisponivel em estoque [{command.Items[0].Name.Trim()}]"));
        }
        #endregion

        #region FINISH COMMAND
        [Fact(DisplayName = "Sucesso ao Finalizar uma Comanda")]
        public async Task Sucesso_ao_finalizar_uma_comanda()
        {
            FinishCommandCommand command = new(
                commandId: _repoCommand.commands[0].CommandId, 
                payment: PaymentEnum.Cash, 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            var result = await _handler.Handle(command, _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Sucesso ao Validar o Metodo de Pagamento")]
        public async Task Sucesso_ao_validar_o_metodo_de_pagamento()
        {
            FinishCommandCommand command = new(
                commandId: _repoCommand.commands[0].CommandId, 
                payment: PaymentEnum.Cash, 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            var result = await _handler.Handle(command, _cancellationToken);
            var commandEntity = (CommandEntity) result.Response;

            Assert.True(command.Payment == commandEntity.Payment);
        }

        [Fact(DisplayName = "Sucesso ao validar a Data do Pagamento da Comanda")]
        public async Task Sucesso_ao_validar_a_data_do_pagamento_da_comanda()
        {
            FinishCommandCommand command = new(
                commandId: _repoCommand.commands[0].CommandId, 
                payment: PaymentEnum.Cash, 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            var result = await _handler.Handle(command, _cancellationToken);
            var commandEntity = (CommandEntity)result.Response;

            Assert.True(DateTime.Now.Date == commandEntity.FinalDate.Value.Date);
        }

        [Fact(DisplayName = "Erro ao Finalizar uma Comanda Inexistente")]
        public async Task Erro_ao_finalizar_uma_comanda_inexistente()
        {
            FinishCommandCommand command = new(
                commandId: 80, 
                payment: PaymentEnum.Cash, 
                companyId: 1,
                discount: 0,
                increase: 0
            );

            var result = await _handler.Handle(command, _cancellationToken);

            Assert.False(IsValid(result, "Comanda não encontrada"));
        }
        #endregion

        #region ADD PRODUCT COMMAND
        [Fact(DisplayName = "Sucesso ao Adicionar Produtos na Comanda")]
        public async Task Sucesso_ao_adicionar_produtos_na_comanda()
        {
            var databaseCommand = new CreateCommandFactory().command;
            var itemsToAdd = databaseCommand.Items.Take(1).ToList();

            //comanda de id 2 e uma comanda sem itens
            AddProductCommandCommand command = new (
                commandId: 2, 
                companyId: 1, 
                items: itemsToAdd
            );

            var result = await _handler.Handle(command, _cancellationToken);

            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.Items.Any(p => p.Name == itemsToAdd[0].Name));
        }

        [Fact(DisplayName = "Sucesso ao Remover Produtos da Comanda")]
        public async Task Sucesso_ao_remover_produtos_da_comanda()
        {
            var databaseCommand = new CreateCommandFactory().command;
            var itemsToAdd = databaseCommand.Items.Take(1).ToList();

            //comanda de id 1 e uma comanda com 4 itens
            AddProductCommandCommand command = new(
                commandId: 1,
                companyId: 1,
                items: itemsToAdd
            );

            var result = await _handler.Handle(command, _cancellationToken);

            var commandEntity = (CommandEntity)result.Response;

            Assert.True(commandEntity.Items.Count == 1);
        }
        #endregion
    }
}
