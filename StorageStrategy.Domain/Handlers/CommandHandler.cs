// using AutoMapper;
// using MediatR;
// using StorageStrategy.Domain.Commands.Command;
// using StorageStrategy.Domain.Repository;
// using StorageStrategy.Models;
// using StorageStrategy.Utils.Helpers;
//
// namespace StorageStrategy.Domain.Handlers
// {
//     public class CommandHandler : HandlerBase,
//         IRequestHandler<CreateCommandCommand, Result>,
//         IRequestHandler<FinishCommandCommand, Result>,
//         IRequestHandler<AddProductCommandCommand, Result>,
//         IRequestHandler<DeleteCommandCommand, Result>,
//         IRequestHandler<RemoveProductCommandCommand, Result>
//     {
//         private IProductRepository _repoProduct;
//         private ICommandRepository _repoCommand;
//         private IEmployeeRepository _repoEmployee;
//         private IMapper _mapper;
//         private IClientRepository _clientRepo;
//
//         public CommandHandler(
//             IProductRepository repoProduct, 
//             ICommandRepository repoCommand, 
//             IEmployeeRepository employeeRepository, 
//             IClientRepository clientRepo,
//             IMapper mapper
//         )
//         {
//             _repoProduct = repoProduct;
//             _mapper = mapper;
//             _repoCommand = repoCommand;
//             _repoEmployee = employeeRepository;
//             _clientRepo = clientRepo;
//         }
//
//         public async Task<Result> Handle(CreateCommandCommand request, CancellationToken cancellationToken)
//         {
//             if (!request.IsValid())
//                 return CreateError(request.GetErros(), "Dados invalido");
//
//             var employee = await _repoEmployee.GetByIdAsync(request.EmployeeId, request.CompanyId);
//
//             if (employee is null)
//                 return CreateError("Funcionario não encontrado");
//
//             var command = request.CreateCommand();
//             
//             command.InitialDate = DateTime.Now.AddHours(-3);
//
//             var result = await HasProductsInStock(request.Items, request.CompanyId);
//
//             if (!result.Success)
//                 return CreateError(result.Errors[0].ErrorMessage);
//
//             var commandItems = request.Items.Select(p => _mapper.Map<CommandItemEntity>(p)).ToList();
//
//             if (request.ClientId == 0)
//             {
//                 command.Name = "Consumidor";
//                 command.ClientId = null;
//                 command.FinalDate = DateTime.Now.AddHours(-3);
//             }
//             else
//             {
//                 var client = await _clientRepo.GetById(request.ClientId);
//                 command.Name = client.Name;
//             }
//     
//             var payments = request.Payments
//                 .Select(_payment => new PaymentEntity(
//                     0,
//                     command.CommandId,
//                     _payment.Method,
//                     _payment.Amount,
//                     _payment.PaymentMethodId,
//                     _payment.TotalFee
//                 )).ToList();
//             
//             command.TotalPrice = commandItems.Sum(p => p.Price * p.Qtd);
//             command.TotalCost = commandItems.Sum(p => p.Cost * p.Qtd);
//             command.TotalTaxing = commandItems.Sum(p => p.Taxing * p.Qtd);
//
//             await _repoCommand.AddAsync(command);
//             await _repoCommand.SaveAsync();
//
//             foreach (var commandItem in commandItems)
//             {
//                 var product = await _repoProduct.GetByIdAsync(commandItem.ProductId, request.CompanyId);
//                 product.Qtd -= commandItem.Qtd;
//                 _repoProduct.Update(product);
//                 await _repoCommand.SaveAsync();
//             }
//             
//             return CreateResponse(command, "Comanda cadastrada com sucesso.");
//         }
//         private async Task<Result> HasProductsInStock(List<CommandItemBase> items, int companyId)
//         {
//             foreach (var itemCommand in items)
//             {
//                 var product = await _repoProduct.GetByIdAsync(itemCommand.ProductId, companyId);
//                 if (product is null)
//                     return new Result(new Error($"Produto não encontrado [{itemCommand.Name.Trim()}]"));
//
//                 if (itemCommand.Qtd > product.Qtd)
//                     return new Result(new Error($"Quantidade indisponivel em estoque [{product.Name.Trim()}]"));
//             }
//
//             return new Result(string.Empty);
//         }
//
//         private async Task<Result> HasProductsInStockCommand(List<CommandItemBase> items, List<CommandItemEntity> itensDb, int companyId)
//         {
//             foreach (var itemCommand in items)
//             {
//                 var product = await _repoProduct.GetByIdAsync(itemCommand.ProductId, companyId);
//                 var itemCommandDb = itensDb.FirstOrDefault(p => p.CommandItemId == itemCommand.CommandItemId);
//
//                 if (itemCommandDb is null || (itemCommand.Qtd < itemCommandDb.Qtd))
//                     continue;
//                 
//                 var difference = Math.Abs(itemCommand.Qtd - itemCommandDb.Qtd);
//                 if (difference > product.Qtd)
//                 {
//                     return new Result(new Error($"Quantidade indisponivel em estoque [{product.Name.Trim()}]"));
//                 }
//             }
//
//             return new Result(string.Empty);
//         }
//
//         public async Task<Result> Handle(FinishCommandCommand request, CancellationToken cancellationToken)
//         {
//             if (!request.IsValid())
//                 return CreateError(request.GetErros(), "Dados inválidos");
//
//             var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
//             if(command is null)
//                 return CreateError("Comanda não encontrada");
//             
//             var payments = request.Payments
//                 .Select(_payment => new PaymentEntity(
//                     0,
//                     command.CommandId,
//                     _payment.Method,
//                     _payment.Amount,
//                     _payment.PaymentMethodId,
//                     _payment.TotalFee
//                 )).ToList();
//             
//             command.Payments.AddRange(payments);
//             command.AddIncrease(request.Increase);
//             command.AddDiscount(request.Discount);
//
//             string messageResponse = "Comanda atualizada com sucesso";
//             if (Calc.CommandHasFinishWithTotalPayments(command))
//             {
//                 command.FinishCommand();
//                 messageResponse =  "Comanda finalizada com sucesso";
//             }
//             
//             _repoCommand.Update(command);
//             await _repoCommand.SaveAsync();
//
//             return CreateResponse(command, messageResponse);
//         }   
//
//         public async Task<Result> Handle(AddProductCommandCommand request, CancellationToken cancellationToken)
//         {
//             if (!request.IsValid())
//                 return CreateError(request.GetErros(), "Dados invalidos.");
//             
//             var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
//             if (command is null)  
//                 return CreateError("Comanda não encontrada");
//
//             var productItems = command.Items;
//             var productsRequest = request.Items.Select(p => _mapper.Map<CommandItemEntity>(p)).ToList();
//             productsRequest.ForEach(p => p.CommandId = command.CommandId);
//
//             var result = await HasProductsInStockCommand(request.Items, productItems, request.CompanyId);
//
//             if (!result.Success)
//                 return CreateError(result.Errors[0].ErrorMessage);
//        
//             foreach (var productRequest in productsRequest)
//             {
//                 var product = await _repoProduct.GetByIdAsync(productRequest.ProductId, request.CompanyId);
//                 if (productRequest.CommandItemId == 0)
//                 {
//                     await AddProductInCommand(product, productRequest);
//                     continue;
//                 }
//                 
//                 var productItemDb = productItems.FirstOrDefault(p => p.CommandItemId == productRequest.CommandItemId);
//                 if (productRequest.Qtd > productItemDb.Qtd)
//                 {
//                     var difference = Math.Abs(productRequest.Qtd - productItemDb.Qtd);
//                     product.Qtd -= difference;
//                     _repoProduct.Update(product);
//                     await _repoProduct.SaveAsync();
//                 }
//                 
//                 if(productItemDb.Qtd > productRequest.Qtd)
//                 {
//                     var difference = Math.Abs(productRequest.Qtd - productItemDb.Qtd);
//                     product.Qtd += difference;
//                     _repoProduct.Update(product);
//                     await _repoProduct.SaveAsync();
//                 }
//                 
//                 productItemDb.Qtd = productRequest.Qtd;
//                 _repoCommand.UpdateCommandItemAsync(productItemDb);
//                 await _repoCommand.SaveAsync();
//                 
//             }
//             
//             command.TotalPrice = productsRequest.Sum(p => p.Price * p.Qtd);
//             command.TotalCost = productsRequest.Sum(p => p.Cost * p.Qtd);
//             _repoCommand.Update(command);
//             await _repoCommand.SaveAsync();
//             
//
//             return CreateResponse(command, "Comanda atualizada com sucesso.");
//         }
//
//         private async Task AddProductInCommand(ProductEntity product, CommandItemEntity productRequest)
//         {
//             product.Qtd -= productRequest.Qtd;
//             var newProductItem = new CommandItemEntity(productRequest.CommandId, productRequest.ProductId,
//                 productRequest.Name, productRequest.Cost, productRequest.Price, productRequest.Qtd);
//                 
//             await _repoCommand.AddItemsAsync(newProductItem);
//         }
//
//         public async Task<Result> Handle(DeleteCommandCommand request, CancellationToken cancellationToken)
//         {
//             if (!request.IsValid())
//                 return CreateError(request.GetErros(), "Dados invalidos");
//
//             var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
//
//             if (command.Items.Count > 0)
//                 return CreateError("Não é possivel excluir uma comanda com produtos");
//
//             _repoCommand.Delete(command);
//             await _repoCommand.SaveAsync();
//
//             return CreateResponse(command, "Comanda removida com sucesso!");
//         }
//
//         public async Task<Result> Handle(RemoveProductCommandCommand request, CancellationToken cancellationToken)
//         {
//             var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
//             var productRemove = command.Items.FirstOrDefault(p => p.ProductId == request.ProductId);
//
//             var product = await _repoProduct.GetByIdAsync(request.ProductId, request.CompanyId);
//             product.Qtd += productRemove.Qtd;
//             _repoProduct.Update(product);
//             await _repoCommand.SaveAsync();
//             await _repoCommand.RemoveCommandItemsAsync(new List<CommandItemEntity>{productRemove});
//             await _repoCommand.SaveAsync();
//             return CreateResponse(true, "Produto removido com sucesso");
//         }
//     }
// }
