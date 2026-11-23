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
    protected IMediator _mediator;

    public CreateCommandHandler(
        IProductRepository repoProduct,
        ICommandRepository repoCommand,
        IEmployeeRepository repoEmployee,
        IMapper mapper,
        IClientRepository clientRepo,
        IMediator mediator
    )
    {
        _repoProduct = repoProduct;
        _repoCommand = repoCommand;
        _repoEmployee = repoEmployee;
        _mapper = mapper;
        _clientRepo = clientRepo;
        _mediator = mediator;
    }

    public async Task<Result> Handle(CreateCommandCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados invalido");

        var employee = await _repoEmployee.GetByIdAsync(request.EmployeeId, request.CompanyId);
        if (employee is null)
            return CreateError("Funcionario não encontrado");

        // Cliente: valida existência antes de qualquer coisa
        string commandName;
        int? clientId;
        if (request.ClientId == 0)
        {
            commandName = "Consumidor";
            clientId = null;
        }
        else
        {
            var client = await _clientRepo.GetById(request.ClientId);
            if (client is null)
                return CreateError("Cliente não encontrado");

            commandName = client.Name;
            clientId = request.ClientId;
        }

        // ATENÇÃO: adapte para o mecanismo de transação real do seu projeto
        // (ex: _repoCommand.BeginTransactionAsync(), _unitOfWork.BeginTransactionAsync(), etc.)
        await using var transaction = await _repoCommand.CreateTranscationAsync(cancellationToken);

        try
        {
            // Trava/valida estoque e recupera os produtos reais do banco
            // (retorna também os produtos, para não confiar em preço/custo vindos do request)
            var stockResult = await ValidateAndLockStockAsync(request.Items, request.CompanyId, cancellationToken);
            if (!stockResult.Success)
            {
                await transaction.RollbackAsync(cancellationToken);
                return CreateError(stockResult.Errors[0].ErrorMessage);
            }

            var productsById = stockResult.Products; // Dictionary<int, ProductEntity>

            // Monta os itens da comanda usando dados oficiais do produto (não do request)
            var commandItems = request.Items.Select(itemRequest =>
            {
                var product = productsById[itemRequest.ProductId];
                var mapped = _mapper.Map<CommandItemEntity>(itemRequest);

                mapped.Price = product.Price;
                mapped.Cost = product.Cost;
                mapped.Taxing = product.Taxing;

                return mapped;
            }).ToList();

            var command = request.CreateCommand();
            command.Name = commandName;
            command.ClientId = clientId;
            command.InitialDate = GetLocalNow();

            // Geração de código sequencial protegida contra concorrência
            // ATENÇÃO: idealmente isso é feito por sequence/identity no banco.
            // Aqui, ao menos, a transação + lock evita leitura suja simultânea.
            var lastCommand = await _repoCommand.GetLastCommandAsync(request.CompanyId);
            command.Code = GenerateNextCode(lastCommand?.Code);

            var payments = request.Payments
                .Select(p => new PaymentEntity(
                    0,
                    command.CommandId,
                    p.Method,
                    p.Amount,
                    p.PaymentMethodId,
                    p.TotalFee
                )).ToList();

            command.TotalPrice = commandItems.Sum(p => p.Price * p.Qtd);
            command.TotalCost = commandItems.Sum(p => p.Cost * p.Qtd);
            command.TotalTaxing = commandItems.Sum(p => p.Taxing * p.Qtd);

            if (payments.Any())
                command.Payments = payments;

            await _repoCommand.AddAsync(command);
            await _repoCommand.SaveAsync();

            // Baixa de estoque
            foreach (var commandItem in commandItems)
            {
                var product = productsById[commandItem.ProductId];
                product.Qtd -= commandItem.Qtd;
                _repoProduct.Update(product);
            }
            await _repoCommand.SaveAsync();

            // Fecha a comanda se o total pago cobre o total (com tolerância de arredondamento)
            var totalPaid = payments.Sum(p => p.Amount);
            var totalCommand = command.TotalPrice - command.Discount + command.Increase;

            if (payments.Any() && IsPaymentComplete(totalPaid, totalCommand))
            {
                command.FinalDate = GetLocalNow();
                _repoCommand.Update(command);
                await _repoCommand.SaveAsync();
            }

            await transaction.CommitAsync(cancellationToken);

            return CreateResponse(command, "Comanda cadastrada com sucesso.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Valida a existência e disponibilidade de estoque dos produtos, retornando
    /// os produtos oficiais do banco (evita confiar em preço/custo enviados pelo request).
    /// Idealmente essa consulta usa lock pessimista (ex: FOR UPDATE) para evitar
    /// condição de corrida entre validação e baixa de estoque.
    /// </summary>
    private async Task<StockValidationResult> ValidateAndLockStockAsync(
        List<CommandItemBase> items,
        int companyId,
        CancellationToken cancellationToken)
    {
        var products = new Dictionary<int, ProductEntity>();

        foreach (var itemCommand in items)
        {
            var product = await _repoProduct.GetByIdAsync(itemCommand.ProductId, companyId);

            if (product is null)
                return StockValidationResult.Fail($"Produto não encontrado [{itemCommand.Name.Trim()}]");

            if (itemCommand.Qtd <= 0)
                return StockValidationResult.Fail($"Quantidade inválida para o produto [{product.Name.Trim()}]");

            if (itemCommand.Qtd > product.Qtd)
                return StockValidationResult.Fail($"Quantidade indisponível em estoque [{product.Name.Trim()}]");

            products[itemCommand.ProductId] = product;
        }

        return StockValidationResult.Ok(products);
    }

    private static string GenerateNextCode(string? lastCode)
    {
        if (string.IsNullOrWhiteSpace(lastCode))
            return "1";

        return int.TryParse(lastCode, out var lastNumericCode)
            ? (lastNumericCode + 1).ToString()
            : "1"; // fallback seguro caso o código legado não seja numérico
    }

    private static bool IsPaymentComplete(decimal totalPaid, decimal totalPrice)
    {
        const decimal tolerance = 0.01m;
        return Math.Abs(totalPaid - totalPrice) <= tolerance;
    }

    private static DateTime GetLocalNow()
    {
        // Preferível a "DateTime.Now.AddHours(-3)" fixo: usa o fuso configurado do sistema.
        // Ajuste o ID do fuso conforme a necessidade (ex: "E. South America Standard Time" no Windows,
        // ou "America/Sao_Paulo" no Linux/macOS).
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }
}

internal class StockValidationResult
{
    public bool Success { get; private init; }
    public List<Error> Errors { get; private init; } = new();
    public Dictionary<int, ProductEntity> Products { get; private init; } = new();

    public static StockValidationResult Ok(Dictionary<int, ProductEntity> products) =>
        new() { Success = true, Products = products };

    public static StockValidationResult Fail(string message) =>
        new() { Success = false, Errors = new List<Error> { new(message) } };
}