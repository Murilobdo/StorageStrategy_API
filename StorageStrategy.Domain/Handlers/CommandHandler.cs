using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class CommandHandler : HandlerBase,
        IRequestHandler<CreateCommandCommand, Result>,
        IRequestHandler<FinishCommandCommand, Result>,
        IRequestHandler<AddProductCommandCommand, Result>,
        IRequestHandler<DeleteCommandCommand, Result>
    {
        private IProductRepository _repoProduct;
        private ICommandRepository _repoCommand;
        private IEmployeeRepository _repoEmployee;
        private IMapper _mapper;

        public CommandHandler(
            IProductRepository repoProduct, 
            ICommandRepository repoCommand, 
            IEmployeeRepository employeeRepository, 
            IMapper mapper
        )
        {
            _repoProduct = repoProduct;
            _mapper = mapper;
            _repoCommand = repoCommand;
            _repoEmployee = employeeRepository;
        }

        public async Task<Result> Handle(CreateCommandCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalido");

            var employee = await _repoEmployee.GetByIdAsync(request.EmployeeId, request.CompanyId);

            if (employee is null)
                return CreateError("Funcionario não encontrado");

            var command = _mapper.Map<CommandEntity>(request);
            command.InitialDate = DateTime.Now.AddHours(-3);

            var result = await HasProductsInStock(request.Items, request.CompanyId);

            if (!result.Success)
                return CreateError(result.Errors[0].ErrorMessage);

            var commandItems = request.Items.Select(p => _mapper.Map<CommandItemEntity>(p)).ToList();

            if (command.Name == "Consumidor")
                command.FinalDate = DateTime.Now.AddHours(-3);

            command.TotalPrice = commandItems.Sum(p => p.Price * p.Qtd);
            command.TotalCost = commandItems.Sum(p => p.Cost * p.Qtd);
            command.TotalTaxing = commandItems.Sum(p => p.Taxing * p.Qtd);

            await _repoCommand.AddAsync(command);
            await _repoCommand.SaveAsync();

            foreach (var commandItem in commandItems)
            {
                var product = await _repoProduct.GetByIdAsync(commandItem.ProductId, request.CompanyId);
                product.Qtd -= commandItem.Qtd;
                _repoProduct.Update(product);
            }
            
            await _repoCommand.SaveAsync();

            return CreateResponse(command, "Comanda cadastrada com sucesso.");
        }

        private async Task<Result> HasProductsInStock(List<CommandItemBase> items, int companyId)
        {
            foreach (var itemCommand in items)
            {
                var product = await _repoProduct.GetByIdAsync(itemCommand.ProductId, companyId);
                if (itemCommand.Qtd > product.Qtd)
                {
                    return new Result(new Error($"Quantidade indisponivel em estoque [{product.Name.Trim()}]"));
                }
            }

            return new Result(string.Empty);
        }

        public async Task<Result> Handle(FinishCommandCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados inválidos");

            var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);

            if(command is null)
                return CreateError("Comanda não encontrada");

            command.FinalDate = DateTime.Now.AddHours(-3);

            var payments = request.Payments
                .Select(p => new PaymentEntity(0, command.CommandId, p.Method, p.Amount))
                .ToList();
            
            command.Payments.AddRange(payments);
            command.Discount = request.Discount;
            command.Increase = request.Increase;

            _repoCommand.Update(command);
            await _repoCommand.SaveAsync();

            return CreateResponse(command, "Comanda finalizada com sucesso.");
        }   

        public async Task<Result> Handle(AddProductCommandCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos.");

            var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);

            if (command is null)  
                return CreateError("Comanda não encontrada");

            var result = await HasProductsInStock(request.Items, request.CompanyId);

            if (!result.Success)
                return CreateError(result.Errors[0].ErrorMessage);

            await _repoCommand.CreateTranscationAsync();

            await ReturnProductsToStock(command.Items, request.CompanyId);
            await _repoCommand.RemoveCommandItemsAsync(command.Items);

            var commandItems = request.Items.Select(p => _mapper.Map<CommandItemEntity>(p)).ToList();
            commandItems.ForEach(p => p.CommandId = command.CommandId);

            await _repoCommand.AddItemsAsync(commandItems);
            await _repoCommand.SaveAsync();

            foreach (var commandItem in commandItems)
            {
                var product = await _repoProduct.GetByIdAsync(commandItem.ProductId, request.CompanyId);
                product.Qtd -= commandItem.Qtd;
                _repoProduct.Update(product);
            }

            command.TotalPrice = commandItems.Sum(p => p.Price * p.Qtd);
            command.TotalCost = commandItems.Sum(p => p.Cost * p.Qtd);

            _repoCommand.Update(command);

            await _repoCommand.SaveAsync();
            await _repoCommand.CommitAsync();

            return CreateResponse(command, "Comanda atualizada com sucesso.");
        }

        private async Task ReturnProductsToStock(List<CommandItemEntity> items, int companyId)
        {
            foreach (var itemCommand in items)
            {
                var product = await _repoProduct.GetByIdAsync(itemCommand.ProductId, companyId);
                product.Qtd += itemCommand.Qtd;
                _repoProduct.Update(product);
            }

            await _repoCommand.SaveAsync();
        }

        public async Task<Result> Handle(DeleteCommandCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);

            if (command.Items.Count > 0)
                return CreateError("Não é possivel excluir uma comanda com produtos");

            _repoCommand.Delete(command);
            await _repoCommand.SaveAsync();

            return CreateResponse(command, "Comanda removida com sucesso!");
        }
    }
}
