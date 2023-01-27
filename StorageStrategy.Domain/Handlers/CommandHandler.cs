using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class CommandHandler : HandlerBase,
        IRequestHandler<CreateCommandCommand, Result>,
        IRequestHandler<UpdateCommandCommand, Result>,
        IRequestHandler<DeleteCommandCommand, Result>,
        IRequestHandler<FinishCommandCommand, Result>
    {

        private IProductRepository _repoProduct;
        private ICommandRepository _repoCommand;
        private IEmployeeRepository _repoEmployee;
        private IMapper _mapper;

        public CommandHandler(IProductRepository repoProduct, ICommandRepository repoCommand, 
            IEmployeeRepository employeeRepository, IMapper mapper)
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
            command.InitialDate = DateTime.Now;

            var commandItems = request.Items.Select(p => _mapper.Map<CommandItem>(p)).ToList();

            if (command.Name == "Consumidor")
                command.FinalDate = DateTime.Now;

            command.TotalPrice = commandItems.Sum(p => p.Price * p.Qtd);
            command.TotalCost = commandItems.Sum(p => p.Cost * p.Qtd);

            await _repoCommand.AddAsync(command);
            await _repoCommand.SaveAsync();


            commandItems.ForEach(p => p.CommandId = command.CommandId);
            await _repoCommand.AddItemsAsync(commandItems);
            await _repoCommand.SaveAsync();

            return CreateResponse(command, "Comanda cadastrada com sucesso.");
        }

        public Task<Result> Handle(UpdateCommandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Handle(DeleteCommandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> Handle(FinishCommandCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados inválidos");

            var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);

            if(command is null)
                return CreateError("Comanda não encontrada");

            command.FinalDate = DateTime.Now;
            command.Payment = request.Payment;

            _repoCommand.Update(command);
            await _repoCommand.SaveAsync();

            return CreateResponse(command, "Comanda finalizada com sucesso.");

        }
    }
}
