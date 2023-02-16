using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Expenses;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class ExpensesHandler : HandlerBase,
        IRequestHandler<CreateExpensesCommand, Result>,
        IRequestHandler<DeleteExpensesCommand, Result>,
        IRequestHandler<CreateExpensesTypeCommand, Result>,
        IRequestHandler<DeleteExpensesTypeCommand, Result>
    {
        private readonly IExpensesRepository _repo;
        private readonly IMapper _mapper;


        public ExpensesHandler(IExpensesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<Result> Handle(CreateExpensesCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var expenses = _mapper.Map<ExpensesEntity>(request);

            await _repo.AddAsync(expenses);
            await _repo.SaveAsync();

            return CreateResponse(expenses, "Despesa cadastrada com sucesso.");
        }

        public async Task<Result> Handle(DeleteExpensesCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var expenses = await _repo.GetExpensesByIdAsync(request.ExpenseId, request.CompanyId);

            if (expenses is null)
                return CreateError("Essa despesa não existe");

            _repo.Delete(expenses);
            await _repo.SaveAsync();

            return CreateResponse(expenses, "Despesa excluida com sucesso.");
        }

        public async Task<Result> Handle(CreateExpensesTypeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var expenseType = await _repo.GetExpensesTypeByDescriptionAsync(request.Description, request.CompanyId);

            if (expenseType is not null)
                return CreateError("Essa despesa ja foi cadastrada");

            await _repo.AddExpenseTypeAsync(expenseType);
            await _repo.SaveAsync();

            return CreateResponse(expenseType, "Despesa cadastrada com sucesso.");
        }

        public async Task<Result> Handle(DeleteExpensesTypeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var expenseType = await _repo.GetExpensesTypeByDescriptionAsync(request.Description, request.CompanyId);

            _repo.DeleteExpenseType(expenseType);
            await _repo.SaveAsync();

            return CreateResponse(expenseType, "Despesa cadastrada com sucesso.");
        }
    }
}
