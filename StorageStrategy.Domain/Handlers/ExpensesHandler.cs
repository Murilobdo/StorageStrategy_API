using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Expenses;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class ExpensesHandler : HandlerBase,
        IRequestHandler<CreateExpenseCommand, Result>,
        IRequestHandler<DeleteExpenseCommand, Result>,
        IRequestHandler<CreateExpenseTypeCommand, Result>,
        IRequestHandler<DeleteExpenseTypeCommand, Result>
    {
        private readonly IExpenseRepository _repo;
        private readonly IMapper _mapper;


        public ExpensesHandler(IExpenseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<Result> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var expenses = _mapper.Map<ExpenseEntity>(request);

            await _repo.AddAsync(expenses);
            await _repo.SaveAsync();

            return CreateResponse(expenses, "Despesa cadastrada com sucesso.");
        }

        public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
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

        public async Task<Result> Handle(CreateExpenseTypeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var expenseType = await _repo.GetExpensesTypeByDescriptionAsync(request.Description, request.CompanyId);

            if (expenseType is not null)
                return CreateError($"Ja existe uma categoria com o nome {expenseType.Description}.");

            expenseType = _mapper.Map<ExpensesTypeEntity>(request);

            await _repo.AddExpenseTypeAsync(expenseType);
            await _repo.SaveAsync();

            return CreateResponse(expenseType, "Categoria de Despesa cadastrada com sucesso.");
        }

        public async Task<Result> Handle(DeleteExpenseTypeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var expenseType = await _repo.GetExpensesTypeByIdAsync(request.ExpenseTypeId, request.CompanyId);

            _repo.DeleteExpenseType(expenseType);
            await _repo.SaveAsync();

            return CreateResponse(expenseType, "Categoria de Despesa excluida com sucesso.");
        }
    }
}
