using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Command;

public class CreateCommandHandler : HandlerBase, IRequestHandler<CreateCommandCommand, Result>
{
    protected IProductRepository _repoProduct;
    protected ICommandRepository _repoCommand;
    protected IEmployeeRepository _repoEmployee;
    protected IMapper _mapper;
    protected IClientRepository _clientRepo;
    
    public CreateCommandHandler(
        IProductRepository repoProduct, 
        ICommandRepository repoCommand, 
        IEmployeeRepository repoEmployee, 
        IMapper mapper, 
        IClientRepository clientRepo) 
    {
        _repoProduct = repoProduct;
        _repoCommand = repoCommand;
        _repoEmployee = repoEmployee;
        _mapper = mapper;
        _clientRepo = clientRepo;
    }

    public async Task<Result> Handle(CreateCommandCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados invalido");

        var employee = await _repoEmployee.GetByIdAsync(request.EmployeeId, request.CompanyId);

        if (employee is null)
            return CreateError("Funcionario não encontrado");

        var command = request.CreateCommand();
        
        command.InitialDate = DateTime.Now.AddHours(-3);

        var result = await HasProductsInStock(request.Items, request.CompanyId);

        if (!result.Success)
            return CreateError(result.Errors[0].ErrorMessage);

        var commandItems = request.Items.Select(p => _mapper.Map<CommandItemEntity>(p)).ToList();

        if (request.ClientId == 0)
        {
            command.Name = "Consumidor";
            command.ClientId = null;
            command.FinalDate = DateTime.Now;
        }
        else
        {
            var client = await _clientRepo.GetById(request.ClientId);
            command.Name = client.Name;
        }

        var payments = request.Payments
            .Select(_payment => new PaymentEntity(
                0,
                command.CommandId,
                _payment.Method,
                _payment.Amount,
                _payment.PaymentMethodId,
                _payment.TotalFee
            )).ToList();
        
        command.TotalPrice = commandItems.Sum(p => p.Price * p.Qtd);
        command.TotalCost = commandItems.Sum(p => p.Cost * p.Qtd);
        command.TotalTaxing = commandItems.Sum(p => p.Taxing * p.Qtd);
        
        if(payments.Any())
            command.Payments = payments;
        
        await _repoCommand.AddAsync(command);
        await _repoCommand.SaveAsync();

        foreach (var commandItem in commandItems)
        {
            var product = await _repoProduct.GetByIdAsync(commandItem.ProductId, request.CompanyId);
            product.Qtd -= commandItem.Qtd;
            _repoProduct.Update(product);
            await _repoCommand.SaveAsync();
        }
        
        return CreateResponse(command, "Comanda cadastrada com sucesso.");
    }
    
    private async Task<Result> HasProductsInStock(List<CommandItemBase> items, int companyId)
    {
        foreach (var itemCommand in items)
        {
            var product = await _repoProduct.GetByIdAsync(itemCommand.ProductId, companyId);
            if (product is null)
                return new Result(new Error($"Produto não encontrado [{itemCommand.Name.Trim()}]"));

            if (itemCommand.Qtd > product.Qtd)
                return new Result(new Error($"Quantidade indisponivel em estoque [{product.Name.Trim()}]"));
        }

        return new Result(string.Empty);
    }
}